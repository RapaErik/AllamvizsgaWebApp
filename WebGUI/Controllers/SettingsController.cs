using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebGUI.Dtos;
using WebGUI.Models;
using WebGUI.SignalRClass;
using DataAccessLayer.IServices;
namespace WebGUI.Controllers
{
    public class SettingsController : BaseController
    {
    
        public SettingsController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService, IDeviceService deviceService) : base(LogService, mapper, chartHubContext, roomService, deviceService)
        {
        }
        public IActionResult Index()
        {

            List<Room> list = _mapper.Map<List<Room>>(_roomService.GetAllRooms());

            return View(list);
        }
        public IActionResult RoomSettings(int id)
        {
            Room room = _mapper.Map<Room>(_roomService.GetRoomById(id));
         

            return View(room);
        }
        public IActionResult UpdateRoom(int id,float? DayliSetpoint,float? NightlySetpoint, string RoomName=null)
        {


            Room room = _mapper.Map<Room>(_roomService.GetRoomById(id));
            _hub.UpdateRoomName(id,RoomName ?? room.Name);
            _hub.UpdateRoomDayliSetpoint(id,DayliSetpoint??room.DailySetpoint);
            _hub.UpdateRoomNightliSetpoint(id, NightlySetpoint ?? room.NightlySetpoint);



            
            return RedirectToAction("RoomSettings", new { @id = id });
        }
    }
}