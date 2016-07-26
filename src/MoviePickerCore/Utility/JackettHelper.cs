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
        private static List<JackettResultGroup> _cache;
        private static DateTime _cacheTime;

        public static IEnumerable<JackettResultGroup> GetResults()
        {
            if (_cache != null && _cacheTime.AddMinutes(5) >= DateTime.Now) return _cache;
            _cache = Query().ToList();
            _cacheTime = DateTime.Now;
            return _cache;
        }

        private static IEnumerable<JackettResultGroup> Query()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Category", "2040")
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

        private const string HighQuality = "brrip|bluray|bdrip|hddvd";
        private const string MidQuality = "web|webrip|dvdrip|web-dl|hdrip|dvdrip";
        private const string LowQuality = "hd-ts|hdcam|hdts|hdtc|hd-tc";

        private static IEnumerable<JackettResult> DeserializeJackettResult(string json)
        {
            var ret = new List<JackettResult>();
            dynamic temp = JsonConvert.DeserializeObject(json);
            var results = temp.Results;
            foreach (var result in results)
            {
                string title = result.Title;
                Match parse = Regex.Match(title, "^(.*)((19|20)[0-9]{2})");
                var quality = 0;
                if (Regex.IsMatch(title, $"{HighQuality}", RegexOptions.IgnoreCase))
                    quality = 3;
                else if (Regex.IsMatch(title, $"{MidQuality}", RegexOptions.IgnoreCase))
                    quality = 2;
                else if (Regex.IsMatch(title, $"{LowQuality}", RegexOptions.IgnoreCase))
                    quality = 1;
                if (parse.Success)
                {
                    ret.Add(new JackettResult
                    {
                        ReleaseTitle = result.Title,
                        Seeders = result.Seeders,
                        Name = FixTitle(parse.Groups[1].ToString()),
                        Year = int.Parse(parse.Groups[2].ToString()),
                        Quality = quality,
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
