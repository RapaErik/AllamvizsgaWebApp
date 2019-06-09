﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebGUI.Models;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogService LogService, IMapper mapper, IHubContext<ChartHub> chartHubContext, IRoomService roomService) : base(LogService, mapper, chartHubContext, roomService)
        {
             //_LogService.InitDatabase();
        }

        public IActionResult Index()
        {
            ViewData["json"] = InitGoogleChart();
            return View();
          
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
