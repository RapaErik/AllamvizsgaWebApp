using System;
using System.Collections.Generic;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using WebGUI.Dtos;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebGUI.Controllers
{
    [Route("api/[controller]")]
    public class ApiSensorDataController : Controller
    {

        private readonly ISensorDataService _sensorDataService;
        private readonly IMapper _mapper;

        public ApiSensorDataController(ISensorDataService sensorDataService, IMapper mapper)
        {
            _mapper = mapper;
            _sensorDataService = sensorDataService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {

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
            return CreatedAtAction("SensorData", new { id=created.Id }, _mapper.Map<SensorData>(created));
        }

    }
}
