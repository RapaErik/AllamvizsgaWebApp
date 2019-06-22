using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebGUI.Models;

namespace WebGUI.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }



        [Authorize]
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Registration(string username, string password)
        {
            var user = new ApplicationUser() { UserName = username, Email = username };
            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return RedirectToAction("Registration", "Login");
            }

        }



        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var result = _signInManager.PasswordSignInAsync(username, password, true, false).Result;
            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }



    }
}

