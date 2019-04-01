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
using WebGUI.Models;
using WebGUI.SignalRClass;

namespace WebGUI.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ISensorDataService sensorDataService, IMapper mapper, IHubContext<ChartHub> chartHubContext) : base(sensorDataService, mapper, chartHubContext)
        {
                
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
