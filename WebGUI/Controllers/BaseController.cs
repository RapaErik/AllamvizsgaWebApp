using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ISensorDataService _sensorDataService;
        protected readonly IMapper _mapper;
        protected readonly IHubContext<ChartHub> _chartHubContext;
        public ChartHub chart;
        protected BaseController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext)
        {
            _mapper = mapper;
            _sensorDataService = sensorDataService;
            _chartHubContext = chartHubContext;
            chart = new ChartHub();

        }
        protected string InitGoogleChart()
        {
            List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatas());
            _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(list));
            //chart.asdasd(JsonConvert.SerializeObject(list));
            return JsonConvert.SerializeObject(list);
        }
    }
}