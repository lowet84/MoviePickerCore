using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using MoviePickerCore.Model;
using Newtonsoft.Json;

namespace MoviePicker.Utility
{
    public static class JackettHelper
    {
        public static IEnumerable<JackettResultGroup> Query(string id)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Category", "2040"),
                    new KeyValuePair<string, string>("Query", id)
                });

                var result = client.PostAsync("http://jackett.local/Admin/search", content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                var deserialized = DeserializeJackettResult(resultContent);
                var ret =
                    deserialized.GroupBy(d => new { d.Name, d.Year })
                        .Select(d => new JackettResultGroup(d.Key.Name, d.Key.Year, d)).ToList();
                ret.Sort((b, a) => a.Results.Sum(e => e.Seeders).CompareTo(b.Results.Sum(f => f.Seeders)));
                return ret;
            }
        }

        private const string HighQuality = "brrip|bluray";
        private const string MidQuality = "web|webrip|dvdrip|web-dl|hdrip";
        private const string LowQuality = "hd-ts|hdcam";

        private static IEnumerable<JackettResult> DeserializeJackettResult(string json)
        {
            var ret = new List<JackettResult>();
            dynamic temp = JsonConvert.DeserializeObject(json);
            var results = temp.Results;
            foreach (dynamic result in results)
            {
                string title = result.Title;
                Match parse = Regex.Match(title, "(.*?)((19|20)[0-9]{2})");
                Match quality = Regex.Match(title, $"({HighQuality}|{MidQuality}|{LowQuality})", RegexOptions.IgnoreCase);
                var qualityText = quality.Success ? quality.Groups[1].ToString() : null;
                if (parse.Success)
                {
                    ret.Add(new JackettResult
                    {
                        ReleaseTitle = result.Title,
                        Seeders = result.Seeders,
                        Name = FixTitle(parse.Groups[1].ToString()),
                        Year = int.Parse(parse.Groups[2].ToString()),
                        Quality = qualityText,
                        Link = result.Link
                    });
                }
            }
            return ret;
        }

        private static string FixTitle(string title)
        {
            var ret = title.Replace(".", " ").Trim();
            if (ret.Substring(ret.Length - 1).Equals("-"))
            {
                ret = ret.Substring(0, ret.Length - 1).Trim();
            }

            return ret;
        }
    }
}
