﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.Controllers
{
    [ResponseCache(Duration = 60 * 60)] // one hour http cache
    public class ErrorController : Controller
    {
        public IActionResult Any()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            if (Response.StatusCode == 200 && !Response.HasStarted)
            {
                Response.StatusCode = 400;
            }
            return View();
        }

        public IActionResult Error404()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            if (Response.StatusCode == 200 && !Response.HasStarted)
            {
                Response.StatusCode = 404;
            }
            return View();
        }
    }
}
