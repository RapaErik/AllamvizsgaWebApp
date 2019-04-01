using System;
using System.Collections.Generic;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.Dtos;
using WebGUI.SignalRClass;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebGUI.Controllers
{
    [Route("api/[controller]")]
    public class ApiSensorDataController : BaseController
    {


        public ApiSensorDataController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext):base( sensorDataService,  mapper,chartHubContext)
        {
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            //  _sensorDataService.ClearSensorDataTable();
            //_sensorDataService.InitDatabase();
            return Ok(_mapper.Map<List<SensorData>>(_sensorDataService.GetSensorDatas()));
        }

        // GET: api/5
        [HttpGet("{id}")]
        public IActionResult GetSomeData(int number)
        {
            return Ok(_mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatas()));
        }

        [HttpPost]
        public IActionResult PostData([FromBody]SensorData data)
        {
            data.TimeStamp=DateTime.Now;
           
            var created = _sensorDataService.InsertSensorData(_mapper.Map<DataAccessLayer.Entities.SensorData>(data));
           
            _chartHubContext.Clients.All.SendAsync("ReceiveMessage", "New Incoming Data", JsonConvert.SerializeObject(data));
            _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(created));
            return CreatedAtAction("SensorData", new { id=created.Id }, _mapper.Map<SensorData>(created));
        }


    }
}
