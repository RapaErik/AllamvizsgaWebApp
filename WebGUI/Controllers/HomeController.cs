using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.Models;
using WebGUI.SignalRClass;
using DataAccessLayer.IServices;
using WebGUI.Dtos;

namespace WebGUI.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(LogService, mapper, chartHubContext, roomService)
        {
           
        }

        public IActionResult Index()
        {
            ViewData["json"] = InitGoogleChart();
            return View();
          
        }
        public IActionResult Home()
        {
            List<Log> logs = _mapper.Map<List<Log>>(_LogService.GetLogsOfTempAndHumi());
            List<RoomAndLogs> roomsAndLogs = new List<RoomAndLogs>();
            List<Room> rooms = new List<Room>();
            foreach (var item in logs)
            {
                rooms.Add(_mapper.Map<Room>(_roomService.GetRoomWhereDeviceIs(item.DeviceId)));
            }

            var disc= rooms.GroupBy(x => x.Id).Select(y => y.First());
            foreach (var item in disc)
            {
                roomsAndLogs.Add(new RoomAndLogs { Room=item, Logs=logs.Where(w=>w.Device.RoomId==item.Id).ToList() });
            }
            return View(roomsAndLogs);

        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
