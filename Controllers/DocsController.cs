using DenizenMetaWebsite.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SharpDenizenTools.MetaHandlers;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.Controllers
{
    public class DocsController : Controller
    {
        public static IActionResult HandleMeta<T>(DocsController controller, Dictionary<string, T> meta) where T : MetaObject
        {
            ThemeHelper.HandleTheme(controller.Request, controller.ViewData);
            DocViewModel model = new DocViewModel()
            {
                IsAll = true,
                CurrentlyShown = meta.Count,
                Max = meta.Count,
                Content = new HtmlString(string.Join(", ", meta.Keys))
            };
            return controller.View(model);
        }

        public IActionResult Index()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Commands()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Commands);
        }
        
        public IActionResult Tags()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Tags);
        }
        
        public IActionResult Events()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Events);
        }
        
        public IActionResult Mechanisms()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Mechanisms);
        }
        
        public IActionResult Actions()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Actions);
        }
        
        public IActionResult Languages()
        {
            return HandleMeta(this, MetaDocs.CurrentMeta.Languages);
        }
    }
}
