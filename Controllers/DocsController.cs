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
            search = search?.ToLowerFast().Replace("%2f", "/");
            List<T> toDisplay = search == null ? objects : objects.Where(o => o.MatchesSearch(search)).ToList();
            List<string> categories = toDisplay.Select(o => o.GroupingString).Distinct().ToList();
            StringBuilder outText = new StringBuilder();
            outText.Append("<center>");
            if (categories.Count > 1)
            {
                outText.Append("<h4>Categories:</h4>");
                outText.Append(string.Join(" | ", categories.Select(category =>
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    return $"<a href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a>";
                })));
                foreach (string category in categories)
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    outText.Append($"<br><hr><br><h4>Category: <a id=\"{linkable}\" href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a></h4><br>");
                    outText.Append(string.Join("\n<br>", toDisplay.Where(o => o.GroupingString == category).Select(o => o.HtmlContent)));
                }
            }
            else
            {
                outText.Append(string.Join("\n<br>", toDisplay.Select(o => o.HtmlContent)));
            }
            outText.Append("</center>");
            DocViewModel model = new DocViewModel()
            {
                IsAll = search == null,
                CurrentlyShown = toDisplay.Count,
                Max = objects.Count,
                Content = new HtmlString(outText.ToString()),
                SearchText = search
            };
            return controller.View(model);
        }

        public IActionResult Index()
        {
            ThemeHelper.HandleTheme(Request, ViewData);
            return View();
        }

        public IActionResult Commands([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Commands);
        }
        
        public IActionResult Tags([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Tags);
        }
        
        public IActionResult Events([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Events);
        }
        
        public IActionResult Mechanisms([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Mechanisms);
        }
        
        public IActionResult Actions([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Actions);
        }
        
        public IActionResult Languages([Bind] string id)
        {
            return HandleMeta(this, id, MetaSiteCore.Languages);
        }
    }
}
