using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.SignalR;
using WebGUI.SignalRClass;
using DataAccessLayer.Entities;
using Newtonsoft.Json;

namespace WebGUI.Controllers
{
    public class GasController : BaseController
    {
        public GasController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(sensorDataService, mapper, chartHubContext)
        { }
        string InitGoogleChart()
        {
            List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatasOfHeater());
            _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(list));
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