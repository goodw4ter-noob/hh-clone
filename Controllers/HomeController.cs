﻿using hh_clone.BL.Auth;
using hh_clone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace hh_clone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICurrentUser currentUser;

        public HomeController(
            ILogger<HomeController> logger,
            ICurrentUser currentUser)
        {
            this.logger = logger;
            this.currentUser = currentUser;
        }

        public async Task<IActionResult> Index()
        {
            var isLoggedIn = await currentUser.IsLoggedIn();

			return View(isLoggedIn);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}