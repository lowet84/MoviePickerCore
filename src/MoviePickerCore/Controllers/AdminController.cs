using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviePicker.Utility;
using MoviePickerCore.Model;
using MoviePickerCore.Utility;

namespace MoviePickerCore.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        public const string ChangeDelugePass = "delugepass";

        // GET: api/values
        [HttpGet]
        public bool Get()
        {
            return true;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public bool Get(string id)
        {
            return true;
        }

        // POST api/values
        [HttpPost]
        public bool Post([FromBody] dynamic data)
        {
            string method = data.method;
            switch (method)
            {
                case ChangeDelugePass:
                    return SettingsHelper.SetPassword((string)data.pass);
                default:
                    return false;
            }
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
