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
            var ret = JackettHelper.Query();
            return ret.Take(10).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IEnumerable<JackettResultGroup> Get(int id)
        {
            var ret = JackettHelper.Query();
            return ret.Take(id).ToList();
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
