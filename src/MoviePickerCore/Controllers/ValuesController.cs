using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MoviePicker.Utility;
using MoviePickerCore.Model;
using MoviePickerCore.Utility;

namespace MoviePickerCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IList<JackettResultGroup> Get()
        {
            return Get(0);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IList<JackettResultGroup> Get(int id)
        {
            var releases = JackettHelper.GetResults().ToList();
            var ret = releases.Skip(id*12).Take(12).ToList();
            foreach (var jackettResultGroup in ret)
            {
                jackettResultGroup.MovieInfo = TheMovieDbHelper.GetMovieInfo(jackettResultGroup.Name,
                    jackettResultGroup.Year);
            }
            return ret;
        }

        // POST api/values
        [HttpPost]
        public bool Post(string link)
        {
            return DelugeHelper.Download(link);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
