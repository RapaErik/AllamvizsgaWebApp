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
    public class ApiSensorDataController : Controller
    {

        private readonly ISensorDataService _sensorDataService;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChartHub> _chartHubContext;
        public ApiSensorDataController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext)
        {
            _mapper = mapper;
            _sensorDataService = sensorDataService;
            _chartHubContext = chartHubContext;
          
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
          //  _sensorDataService.ClearSensorDataTable();
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
           
            var created = _sensorDataService.InsertSensorData(_mapper.Map<DataAccessLayer.Entities.SensorData>(data));
            _chartHubContext.Clients.All.SendAsync("ReceiveMessage", "New Incoming Data", created.ToString());
            _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(data));
            return CreatedAtAction("SensorData", new { id=created.Id }, _mapper.Map<SensorData>(created));
        }

    }
}
