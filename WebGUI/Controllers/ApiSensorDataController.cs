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
    [Route("api/[controller]/[action]")]
    public class ApiSensorDataController : BaseController
    {


        public ApiSensorDataController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) :base( sensorDataService,  mapper,chartHubContext,  roomService)
        {
        }
        
        
        [HttpGet]
        public IActionResult Get()
        {
            // _sensorDataService.ClearSensorDataTable();
            //_sensorDataService.InitDatabase();
            return Ok(_mapper.Map<List<SensorData>>(_sensorDataService.GetLastNSensorDatasBySensorType()));
        }


        [HttpGet]
        public IActionResult GetLastNSensorDatasBySensorType(string datatype = "", int? number = null, int? sensorId = null)
        {
            
                return Ok(_sensorDataService.GetLastNSensorDatasBySensorType(datatype, number, sensorId));
            
        }

        [HttpPost]
        public IActionResult PostData([FromBody]SensorData data)
        {
            var created = _sensorDataService.InsertSensorData(_mapper.Map<DataAccessLayer.Entities.SensorData>(data));

            _hub.SendToRestApiMsg(JsonConvert.SerializeObject(created));

            return CreatedAtAction("SensorData", new { id=created.Id }, _mapper.Map<SensorData>(created));
        }

        

    }
}
