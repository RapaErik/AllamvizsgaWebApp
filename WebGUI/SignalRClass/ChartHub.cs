using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGUI.Dtos;

namespace WebGUI.SignalRClass
{
    public class ChartHub : Hub
    {

        protected readonly IRoomService _roomService;
        protected readonly IMapper _mapper;
        IHubContext<ChartHub> _chartHubContext;
        public ChartHub(IRoomService roomService, IMapper mapper , IHubContext<ChartHub> ctx)
        {
            _mapper = mapper;
            _roomService = roomService;
            _chartHubContext = ctx;


        }
        public void AddNewRoom()
        {
            _roomService.AddNewRoom();
            return;
        }

        public void SendToRestApiMsg(string json)
        {
             var t = _chartHubContext.Clients.All.SendAsync("RestApiMsg", json);
            t.Dispose();
        }
    }
}
