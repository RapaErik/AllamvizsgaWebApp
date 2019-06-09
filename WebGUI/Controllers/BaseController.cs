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
        protected readonly ILogService _LogService;
        protected readonly IRoomService _roomService;
        protected readonly IMapper _mapper;
        protected readonly IHubContext<ChartHub> _chartHubContext;
        public ChartHub _hub;
        protected BaseController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService, IDeviceService deviceService=null)
        {
            _mapper = mapper;
            _LogService = LogService;
            _chartHubContext = chartHubContext;
            _roomService = roomService;
             
            _hub = new ChartHub(_roomService, _mapper, _chartHubContext,deviceService);


        }
        protected string InitGoogleChart()
        {
            List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastFiftyLogsExceptHeaters());
            _hub.SendToRestApiMsg(JsonConvert.SerializeObject(list));
            //_chartHubContext.Clients.All.SendAsync("RestApiMsg", JsonConvert.SerializeObject(list));
        
            return JsonConvert.SerializeObject(list);
        }

    }
}