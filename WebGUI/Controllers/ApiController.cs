using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebGUI.Controllers
{
    [Route("api/")]
    public class ApiController : Controller
    {


        private readonly HeatingContext _ctx;
        public ApiController(HeatingContext ctx)
        {
            _ctx = ctx;
            if (_ctx.SensorDatas.Count() == 0)
            {
                _ctx.SensorDatas.Add(new SensorData { Data = "0", TimeStamp = DateTime.Now, SensorId=1 });
                _ctx.SaveChanges();
            }
        }

        // GET: api/
        [HttpGet]
        public async Task<IEnumerable<SensorData>> GetAllSensorDatas()
        {
            return await _ctx.SensorDatas.ToListAsync();

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
    }
}
