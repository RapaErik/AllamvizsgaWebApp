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

namespace WebGUI.Controllers
{
    public class GasController : BaseController
    {
        public GasController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(sensorDataService, mapper, chartHubContext, roomService)
        { }
        protected new string InitGoogleChart()
        {
            //List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatasOfHeater());
            List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastNSensorDatasBySensorType("heater",50));
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