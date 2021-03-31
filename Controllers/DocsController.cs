using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.Controllers
{
    public class DocsController : Controller
    {
        public IActionResult Index()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Commands()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Tags()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Events()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Mechanisms()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Actions()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Languages()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
    }
}
