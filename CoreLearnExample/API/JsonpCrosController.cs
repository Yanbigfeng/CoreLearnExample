using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Common;
using CoreLearnExample.Filter;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreLearnExample.API
{
    [Route("api/[controller]")]
    public class JsonpCrosController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [ServiceFilter(typeof(JsonpResultFilter))]
        //[Route("api/[controller]/GetValue")]
        [HttpGet("GetValue")]
        [JsonpResultFilter]
        //[HttpGet]
        public string GetValue(string isCallback)
        {
            
            var str = "";

            return "这是GetVlue返回的值";

        }
    }
}
