using AutoMapper;
using DataAccessLayer.IServices;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebGUI.Dtos;
using WebGUI.SignalRClass;



namespace WebGUI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ApiLogController : BaseController
    {


        public ApiLogController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService, IDeviceService deviceService, ICommunicationUnitService communicationUnitService) : base(LogService, mapper, chartHubContext, roomService, deviceService, communicationUnitService)
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

            return Ok(_mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType(datatype, number, DeviceId)));

        }

        [HttpGet]
        public IActionResult GetListOfDevices(int? roomId)
        {
            return Ok(_mapper.Map<List<Device>>(_deviceService.GetListOfDevices(roomId)));
        }

        [HttpGet]
        public IActionResult GetListOfRooms(int? roomId)
        {
            return Ok(_mapper.Map<List<Room>>(_roomService.GetAllRooms(roomId)));
        }

        [HttpPost]
        public IActionResult PostData([FromBody]Log data)
        {
            var created = _LogService.InsertLog(_mapper.Map<DataAccessLayer.Entities.Log>(data));

            _hub.SendToRestApiMsg(JsonConvert.SerializeObject(created));

            return CreatedAtAction("Log", new { id = created.Id }, _mapper.Map<Log>(created));
        }

        [HttpPost]
        public IActionResult InitNewCommunicationUnit([FromBody]string ipAddress)
        {
            int insertedId = _communicationUnitService.InsertNewCommunicationUnit(ipAddress);
            return CreatedAtAction("InitNewCommunicationUnit", insertedId);

        }
    }
}
