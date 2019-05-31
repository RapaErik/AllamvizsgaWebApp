using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.Dtos;
using WebGUI.Models;
using WebGUI.SignalRClass;



namespace WebGUI.Controllers
{
    [Route("api/[controller]")]
    public class ApiSensorDataController : BaseController
    {


        public ApiSensorDataController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) :base( sensorDataService,  mapper,chartHubContext,  roomService)
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

        [HttpGet("lasttempdata")]
        public IActionResult GetLastTempData()
        {
            return Ok(_sensorDataService.GetLastTemperatureSensorData());
        }

        [HttpPost]
        public IActionResult PostData([FromBody]SensorData data)
        {
            //  data.TimeStamp=DateTime.Now;
            //  data.TimeStamp=DateTime.Now;

            var created = _sensorDataService.InsertSensorData(_mapper.Map<DataAccessLayer.Entities.SensorData>(data));

              _hub.SendToRestApiMsg(JsonConvert.SerializeObject(created));
            //Task.Run<ChartHub>(async () => await _hub.SendToRestApiMsg(JsonConvert.SerializeObject(created)));
           
           // _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(created));
            return CreatedAtAction("SensorData", new { id=created.Id }, _mapper.Map<SensorData>(created));
        }



    }
}
