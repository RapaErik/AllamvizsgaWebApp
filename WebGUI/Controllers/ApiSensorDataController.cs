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
    public class ApiLogController : BaseController
    {


        public ApiLogController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) :base( LogService,  mapper,chartHubContext,  roomService)
        {
        }
        
        
        [HttpGet]
        public IActionResult Get()
        {
            // _LogService.ClearLogTable();
            //_LogService.InitDatabase();
            return Ok(_mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType()));
        }


        [HttpGet]
        public IActionResult GetLastNLogsByDeviceType(string datatype = "", int? number = null, int? DeviceId = null)
        {
            
                return Ok(_LogService.GetLastNLogsByDeviceType(datatype, number, DeviceId));
            
        }

        [HttpPost]
        public IActionResult PostData([FromBody]Log data)
        {
            var created = _LogService.InsertLog(_mapper.Map<DataAccessLayer.Entities.Log>(data));

            _hub.SendToRestApiMsg(JsonConvert.SerializeObject(created));

            return CreatedAtAction("Log", new { id=created.Id }, _mapper.Map<Log>(created));
        }

        

    }
}
