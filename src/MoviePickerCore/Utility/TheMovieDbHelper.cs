using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MoviePickerCore.Model;
using Newtonsoft.Json;

namespace MoviePickerCore.Utility
{
    public static class TheMovieDbHelper
    {
        private static Dictionary<string, MovieInfo> _cache;

        public static MovieInfo GetMovieInfo(string name, int year)
        {
            if (_cache == null)
            {
                _cache = new Dictionary<string, MovieInfo>();
            }
            if (_cache.ContainsKey($"{name}#{year}"))
            {
                return _cache[$"{name}#{year}"];
            }

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Category", "2040")
                });

                var result = client.GetAsync($"http://api.themoviedb.org/3/search/movie?api_key=77238c64f20d9bde098fe2b0e5c88141&query={name}").Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                dynamic deserialized = JsonConvert.DeserializeObject(resultContent);
                foreach (var item in deserialized.results)
                {
                    string imageUrl = $"https://image.tmdb.org/t/p/w500{item.poster_path}";
                    string releaseDate = item.release_date.ToString();
                    if (releaseDate.Contains(year.ToString()))
                    {
                        var ret = new MovieInfo
                        {
                            ImageUrl = imageUrl,
                            Overview = item.overview
                        };
                        if (!_cache.ContainsKey($"{name}#{year}"))
                            _cache.Add($"{name}#{year}", ret);
                        return ret;
                    }
                }
                return null;
            }
        }
    }
}
