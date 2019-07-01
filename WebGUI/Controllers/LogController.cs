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
using DataAccessLayer.IServices;
using Microsoft.AspNetCore.Authorization;

namespace WebGUI.Controllers
{
    [Authorize]
    public class LogController : BaseController
    {
        public LogController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(LogService, mapper, chartHubContext) { }

        public IActionResult Index()
        {

            ViewData["pager_size"] = (int)Math.Ceiling((decimal)_LogService.GetDbSize() / (decimal)50);
            //List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastFiftyLogsSortedByTime());
            List<Log> list = _mapper.Map<List<Log>>(_LogService.GetLastNLogsByDeviceType("", 50));
            ViewData["page"] = 1;
            return View(list);
        }
        public IActionResult Filtering(string roomName, DateTime startDate, DateTime endDate, int page, bool temp, bool humidity, bool error, bool cevent, bool hevent)
        {


            string type = temp ? "temperature-" : "";
            type += humidity ? "humidity-" : "";
            type += error ? "error-" : "";
            type += cevent ? "cooler-" : "";
            type += hevent ? "heater-" : "";

            if (type == "" && endDate == DateTime.MinValue && startDate == DateTime.MinValue && page == 1 && roomName == "")
            {
                RedirectToAction("Index");
            }
            if (endDate == DateTime.MinValue)
            {
                endDate = DateTime.Now;
            }




            List<Log> list = _mapper.Map<List<Log>>(_LogService.Filter(startDate, endDate, page, type, roomName));
            ViewData["pager_size"] = (int)Math.Ceiling((decimal)_LogService.GetFiltredDbSize(startDate, endDate, type, roomName) / (decimal)50);


            ViewData["temp"] = temp ? "checked" : "";
            ViewData["humidity"] = humidity ? "checked" : "";
            ViewData["error"] = error ? "checked" : "";
            ViewData["cevent"] = cevent ? "checked" : "";
            ViewData["hevent"] = hevent ? "checked" : "";
            ViewData["startDate"] = startDate.Year+"-"+ (startDate.Month < 10? "0":"")+ startDate.Month+"-"+(startDate.Day < 10 ? "0" : "") + startDate.Day;
            ViewData["endDate"] = endDate.Year+"-" +(endDate.Month < 10 ? "0" : "")+endDate.Month+"-" + (endDate.Day < 10 ? "0" : "")+endDate.Day;
            ViewData["roomName"] = roomName;
            ViewData["page"] = page;
            return View("Index", list);
        }
    }
}