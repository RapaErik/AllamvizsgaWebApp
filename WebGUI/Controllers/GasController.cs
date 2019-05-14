using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.SignalR;
using WebGUI.SignalRClass;
namespace WebGUI.Controllers
{
    public class GasController : BaseController
    {
        public GasController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(sensorDataService, mapper, chartHubContext)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}