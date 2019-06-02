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
        public LogController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(sensorDataService, mapper, chartHubContext, roomService) { }

        public IActionResult Index()
        {

            //List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatasSortedByTime());
            List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastNSensorDatasBySensorType("",50));

            return View(list);
        }

    }
}