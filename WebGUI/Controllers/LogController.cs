using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.Dtos;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class LogController : BaseController
    {
        public LogController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(LogService, mapper, chartHubContext, roomService) { }

        public IActionResult Index()
        {

            //List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastFiftyLogsSortedByTime());
            List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType("",50));

            return View(list);
        }

    }
}