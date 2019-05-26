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
    public class BaseController : Controller
    {
        protected readonly ISensorDataService _sensorDataService;
        protected readonly IRoomService _roomService;
        protected readonly IMapper _mapper;
        protected readonly IHubContext<ChartHub> _chartHubContext;
        public ChartHub _hub;
        protected BaseController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService)
        {
            _mapper = mapper;
            _sensorDataService = sensorDataService;
            _chartHubContext = chartHubContext;
            _roomService = roomService;
            
            _hub = new ChartHub(_roomService, _mapper, _chartHubContext);

        }
        protected string InitGoogleChart()
        {
            List<SensorData> list = _mapper.Map<List<SensorData>>(_sensorDataService.GetLastFiftySensorDatasExceptHeaters());
            _chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(list));
            
            //chart.asdasd(JsonConvert.SerializeObject(list));
            return JsonConvert.SerializeObject(list);
        }

    }
}