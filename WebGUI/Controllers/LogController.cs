using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class LogController : BaseController
    {
        public LogController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(sensorDataService, mapper, chartHubContext)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}