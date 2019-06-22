using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.SignalR;
using WebGUI.SignalRClass;

using Newtonsoft.Json;
using WebGUI.Dtos;
using DataAccessLayer.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebGUI.Controllers
{
    [Authorize]
    public class GasController : BaseController
    {
        public GasController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(LogService, mapper, chartHubContext)
        { }
        protected new string InitGoogleChart()
        {
            //List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastFiftyLogsOfHeater());
            List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType("heater",50));
           // List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType("heater"));
            _hub.SendToRestApiMsg(JsonConvert.SerializeObject(list));
            //_chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(list));
            //chart.asdasd(JsonConvert.SerializeObject(list));
            return JsonConvert.SerializeObject(list);
        }
        public IActionResult Index()
        {
            ViewData["json"] = InitGoogleChart();
            return View();

        }
    }
}