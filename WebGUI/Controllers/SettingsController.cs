using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebGUI.Dtos;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class SettingsController : BaseController
    {
        public SettingsController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(sensorDataService, mapper, chartHubContext, roomService)
        { }
        public IActionResult Index()
        {

            List<Room> list = _mapper.Map<List<Room>>(_roomService.GetAllRooms());

            return View(list);
        }
        public IActionResult RoomSettings(int Id)
        {

           Room item = _mapper.Map<Room>(_roomService.GetRoomById(Id));

            return View(item);
        }

    }
}