﻿using Microsoft.AspNetCore.Mvc;

namespace E_Ticket_Stor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
