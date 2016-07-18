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
        public string Quality { get; set; }
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

        public JackettResultGroup(string name, int year, IEnumerable<JackettResult> results)
        {
            Name = name;
            Year = year;
            Results = results;
            Seeders = results.Sum(d => d.Seeders);
        }

        public override string ToString()
        {
            return $"{Name} ({Results.Count()}) [{Results.Sum(d => d.Seeders)}]";
        }
    }
}
