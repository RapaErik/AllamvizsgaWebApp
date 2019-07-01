using AutoMapper;
using DataAccessLayer.IServices;
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
        protected readonly IDeviceService _deviceService;
        protected readonly IMapper _mapper;
        IHubContext<ChartHub> _chartHubContext;
        public ChartHub(IRoomService roomService, IMapper mapper, IHubContext<ChartHub> ctx, IDeviceService deviceService)
        {
            _mapper = mapper;
            _roomService = roomService;
            _deviceService = deviceService;
            _chartHubContext = ctx;


        }

        public void AddNewRoom()
        {
            var r= _mapper.Map<Room>(_roomService.AddNewRoom());
            var t = _chartHubContext.Clients.All.SendAsync("NewRoomId", r.Id);
            t.Dispose();
        }
        public void GettAllFreeEsps()
        {
            List<Device> DevicesList = _mapper.Map<List<Device>>(_deviceService.GetAllDevicesWithoutOfRooms());

            var json = JsonConvert.SerializeObject(DevicesList);

            var t = _chartHubContext.Clients.All.SendAsync("GettingEsps", json);
            t.Dispose();
        }
        public void GetEspsOfRoomInvoke(int id)
        {
            List<Device> DevicesListOfRoom = _mapper.Map<List<Device>>(_deviceService.GetAllDevicesRoomId(id));
            var json = JsonConvert.SerializeObject(DevicesListOfRoom);

            var t = _chartHubContext.Clients.All.SendAsync("GettingEspsDisplay", json);
            t.Dispose();
        }

        public void RemoveEspFromRoom(int espId)
        {
            _deviceService.RemoveEspFromRoom(espId);
        }
        public void SendToRestApiMsg(string json)
        {
            var t = _chartHubContext.Clients.All.SendAsync("RestApiMsg", json);
            if (t != null && t.Status == TaskStatus.RanToCompletion)
            {
                t.Dispose();
            }

        }

        public void AddEspToRoom(int roomId, int espId)
        {
            List<Device> DevicesListOfRoom = _mapper.Map<List<Device>>(_deviceService.AddEspToRoom(roomId, espId));
            var json = JsonConvert.SerializeObject(DevicesListOfRoom);


            var t = _chartHubContext.Clients.All.SendAsync("GettingEspsDisplay", json);
            t.Dispose();
        }

        public void UpdateRoomName(int roomId, string roomName)
        {
            _roomService.UpdateRoomName(roomId, roomName);
        }
        public void UpdateRoomNightliSetpoint(int roomId, float setpoint)
        {
            _roomService.UpdateRoomNightliSetpoint(roomId, setpoint);
        }
        public void UpdateRoomDayliSetpoint(int roomId, float setpoint)
        {
            _roomService.UpdateRoomDayliSetpoint(roomId, setpoint);
        }

        public void RoomHeaterCoolerToggle(int roomId, string textContent)
        {
            if (textContent.Contains("Heater"))
            {
                _roomService.ToggleHeaterByRoomId(roomId);
            }
            else
            {
                _roomService.ToggleCoolerByRoomId(roomId);
            }
          
        }
    }

}
