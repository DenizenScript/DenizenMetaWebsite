using DenizenMetaWebsite.MetaObjects;
using DenizenMetaWebsite.Models;
using FreneticUtilities.FreneticExtensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SharpDenizenTools.MetaHandlers;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DenizenMetaWebsite.Controllers
{
    public class DocsController : Controller
    {
        public static IActionResult HandleMeta<T>(DocsController controller, string search, List<T> objects) where T : WebsiteMetaObject
        {
            ThemeHelper.HandleTheme(controller.Request, controller.ViewData);
            search = search?.ToLowerFast();
            List<T> toDisplay = search == null ? objects : objects.Where(o => o.MatchesSearch(search)).ToList();
            List<string> categories = toDisplay.Select(o => o.GroupingString).Distinct().ToList();
            StringBuilder outText = new StringBuilder();
            if (categories.Count > 1)
            {
                outText.Append("<center><h4>Categories:</h4>");
                foreach (string category in categories)
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    outText.Append($"<a href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a>\n<br>");
                }
                outText.Append("</center>");
                foreach (string category in categories)
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    outText.Append($"<center><h4>Category: <a id=\"{linkable}\" href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a></h4></center>");
                    outText.Append(string.Join("\n<br>", toDisplay.Where(o => o.GroupingString == category).Select(o => o.HtmlContent)));
                }
            }
            else
            {
                outText.Append(string.Join("\n<br>", toDisplay.Select(o => o.HtmlContent)));
            }
            DocViewModel model = new DocViewModel()
            {
                IsAll = search == null,
                CurrentlyShown = toDisplay.Count,
                Max = objects.Count,
                Content = new HtmlString(outText.ToString())
            };
            return controller.View(model);
        }

        public IActionResult Index()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }
        
        public IActionResult Commands(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Commands);
        }
        
        public IActionResult Tags(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Tags);
        }
        
        public IActionResult Events(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Events);
        }
        
        public IActionResult Mechanisms(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Mechanisms);
        }
        
        public IActionResult Actions(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Actions);
        }
        
        public IActionResult Languages(string search)
        {
            return HandleMeta(this, search, MetaSiteCore.Languages);
        }
    }
}
