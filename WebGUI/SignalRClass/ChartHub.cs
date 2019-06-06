using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
        protected readonly ISensorService _sensorService;
        protected readonly IMapper _mapper;
        IHubContext<ChartHub> _chartHubContext;
        public ChartHub(IRoomService roomService, IMapper mapper, IHubContext<ChartHub> ctx, ISensorService sensorService)
        {
            _mapper = mapper;
            _roomService = roomService;
            _sensorService = sensorService;
            _chartHubContext = ctx;


        }

        public void AddNewRoom()
        {
            _roomService.AddNewRoom();
            return;
        }
        public void GettAllFreeEsps()
        {
            List<Sensor> sensorsList = _mapper.Map<List<Sensor>>(_sensorService.GetAllSensorsWithoutOfRooms());
        
            var json = JsonConvert.SerializeObject(sensorsList);

            var t = _chartHubContext.Clients.All.SendAsync("GettingEsps", json);
            t.Dispose();
        }
        public void GetEspsOfRoomInvoke(int id)
        {
            List<Sensor> sensorsListOfRoom = _mapper.Map<List<Sensor>>(_sensorService.GetAllSensorsRoomId(id));
            var json = JsonConvert.SerializeObject(sensorsListOfRoom);

            var t = _chartHubContext.Clients.All.SendAsync("GettingEspsDisplay", json);
            t.Dispose();
        }

        public void RemoveEspFromRoom(int espId)
        {
            _sensorService.RemoveEspFromRoom(espId);
        }
        public void SendToRestApiMsg(string json)
        {
            var t = _chartHubContext.Clients.All.SendAsync("RestApiMsg", json);
            t.Dispose();
        }
    }
}
