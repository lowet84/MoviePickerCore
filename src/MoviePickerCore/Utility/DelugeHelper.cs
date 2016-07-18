using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MoviePickerCore.Utility
{
    public static class DelugeHelper
    {
        private const string Url = "http://deluge.local/json";
        private const string Location = "/mnt/core/Film";

        public static bool Download(string link)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = true,
                CookieContainer = cookieContainer
            };
            using (var client = new HttpClient(handler))
            {
                var pass = SettingsHelper.GetDelugePass();
                return Login(client, pass) && Download(client, link, cookieContainer);
            }
        }

        private static bool Login(HttpClient client, string pass)
        {
            var temp = PostToDelugeWithParameters(client, "auth.login", pass);
            var ret = bool.Parse(temp.result.ToString());
            return ret;
        }

        private static dynamic PostToDelugeWithParameters(HttpClient client, string method, params string[] parameters)
        {
            return PostToDeluge(client, method, $"['{string.Join("','", parameters)}']");
        }

        private static dynamic PostToDeluge(HttpClient client, string method ,string parameters)
        {
            var content = $@"
            {{
                'method':'{method}',
                'id':'0',
                'params':{parameters}
            }}
            ".Replace("'", "\"");
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var result = client.PostAsync(Url, stringContent).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            dynamic temp = JsonConvert.DeserializeObject(resultContent);
            return temp;
        }

        private static bool Download(HttpClient client, string link, CookieContainer cookieContainer)
        {
            var cookies = cookieContainer.GetCookies(new Uri(Url)).Cast<Cookie>().ToList();
            string tempFile = PostToDelugeWithParameters(client, "web.download_torrent_from_url",link,string.Join(",",cookies.Select(d=>d.ToString()))).result.ToString();
            var content = $"[[{{'path': '{tempFile}','options': {{ 'download_location': '{Location}'}}}}]]".Replace("'", "\"");
            var downloadResult = PostToDeluge(client,"web.add_torrents", content);
            var ret = bool.Parse(downloadResult.result.ToString());
            return ret;
        }
    }
}
