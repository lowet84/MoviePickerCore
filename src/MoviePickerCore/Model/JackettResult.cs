using System.Collections.Generic;
using System.Linq;

namespace MoviePickerCore.Model
{
    public class JackettResult
    {
        public string ReleaseTitle { get; set; }
        public int Seeders { get; set; }

        public int Year { get; set; }

        public string Name { get; set; }
        public int Quality { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return $"{Name}[{Year}] ({Seeders}) {Quality}";
        }
    }

    public class JackettResultGroup
    {
        public string Name { get; }
        public int Year { get; }
        public int Seeders { get; set; }
        public IEnumerable<JackettResult> Results { get; }
        public int Quality { get; set; }
        public MovieInfo MovieInfo { get; set; }

        public JackettResultGroup(string name, int year, IEnumerable<JackettResult> results)
        {
            Name = name;
            Year = year;
            Results = results.ToList();
            Seeders = Results.Sum(d => d.Seeders);
            var quality = Results.Max(d => d.Quality);
            Quality = quality * ((quality > 0) && Results.Any(d => d.Quality == 0) ? -1 : 1);
        }

        public override string ToString()
        {
            return $"{Name} ({Results.Count()}) [{Results.Sum(d => d.Seeders)}]";
        }
    }
}
