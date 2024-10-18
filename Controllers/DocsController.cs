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
    [ResponseCache(Duration = 60 * 10)] // 10 minute http cache
    public class DocsController : Controller
    {
        public static IActionResult HandleMeta<T>(DocsController controller, string search, List<T> objects, Func<List<T>, List<T>> extraSearchResults = null) where T : WebsiteMetaObject
        {
            ThemeHelper.HandleTheme(controller.Request, controller.ViewData);
            List<T> toDisplay = search == null ? objects : objects.Where(o => o.ObjectGeneric.SearchHelper.GetMatchQuality(search) > 6).ToList();
            List<T> extra = extraSearchResults?.Invoke(toDisplay);
            if (extra is not null)
            {
                toDisplay.InsertRange(0, extra);
            }
            if (toDisplay.IsEmpty())
            {
                Console.WriteLine($"Search for '{search}' found 0 results");
            }
            T exactMatch = toDisplay.FirstOrDefault(o => o.ObjectGeneric.CleanName == search);
            if (exactMatch is not null)
            {
                toDisplay.Clear();
                toDisplay.Add(exactMatch);
            }
            List<string> categories = toDisplay.Select(o => o.GroupingString ?? "(ERROR MISSING GROUP)").Distinct().ToList();
            StringBuilder outText = new();
            outText.Append("<center>\n");
            if (categories.Count > 1)
            {
                outText.Append("<h4>Categories:</h4><div class=\"categories_container\">\n");
                outText.Append(string.Join(" | ", categories.Select(category =>
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    return $"<a href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a>\n";
                })));
                outText.Append("</div>\n");
                foreach (string category in categories)
                {
                    string linkable = HttpUtility.UrlEncode(category.ToLowerFast());
                    outText.Append($"<br><hr><br><h4>Category: <a id=\"{linkable}\" href=\"#{linkable}\" onclick=\"doFlashFor('{linkable}')\">{Util.EscapeForHTML(category)}</a></h4><br>\n");
                    outText.Append(string.Join("\n<br>", toDisplay.Where(o => o.GroupingString ?? "(ERROR MISSING GROUP)" == category).Distinct().Select(o => o.HtmlContent)));
                }
            }
            else
            {
                outText.Append(string.Join("\n<br>", toDisplay.Select(o => o.HtmlContent)));
            }
            outText.Append("</center>");
            DocViewModel model = new()
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

        /// <summary>Inexplicably, ASP.NET does not parse URL encoding input, so, this is needed I guess.</summary>
        public static string FixID(string id)
        {
            if (id == null)
            {
                return null;
            }
            return id.ToLowerFast().Replace('+', ' ').Replace("%2f", "/");
        }

        public IActionResult Commands([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id, MetaSiteCore.Commands);
        }

        public IActionResult ObjectTypes([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id, MetaSiteCore.ObjectTypes);
        }

        public IActionResult Tags([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id is null ? null : MetaTag.CleanTag(id), MetaSiteCore.Tags);
        }

        private static List<WebsiteMetaEvent> GetExtraEvents(string id)
        {
            if (id is not null)
            {
                List<(MetaEvent, int)> evts = MetaDocs.CurrentMeta.GetEventMatchesFor(id, true, false);
                if (evts is not null && evts.Any(p => p.Item2 > 1))
                {
                    IEnumerable<(MetaEvent, int)> betterMatches = evts.Where(p => p.Item1.CleanEvents.Any(e => e.Contains(id)));
                    if (betterMatches.Any())
                    {
                        evts = betterMatches.ToList();
                    }
                    int best = evts.Max(p => p.Item2);
                    IEnumerable<MetaEvent> results = evts.Where(p => p.Item2 >= best - 2).OrderByDescending(p => p.Item2).Select(p => p.Item1);
                    return results.Select(e => MetaSiteCore.Events.First(we => we.Object == e)).ToList(); // TODO: This is a dirty lookup hack
                }
            }
            return null;
        }

        public IActionResult Events([Bind] string id)
        {
            id = FixID(id);
            List<WebsiteMetaEvent> addedEvent = [];
            return HandleMeta(this, id, MetaSiteCore.Events, (orig) => orig.IsEmpty() ? GetExtraEvents(id) : null);
        }

        public IActionResult Mechanisms([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id, MetaSiteCore.Mechanisms);
        }

        public IActionResult Actions([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id, MetaSiteCore.Actions);
        }

        public IActionResult Languages([Bind] string id)
        {
            id = FixID(id);
            return HandleMeta(this, id, MetaSiteCore.Languages);
        }

        public IActionResult Search([Bind] string id)
        {
            id = FixID(id);
            ThemeHelper.HandleTheme(Request, ViewData);
            if (string.IsNullOrWhiteSpace(id))
            {
                id = "Nothing";
            }
            List<(int, WebsiteMetaObject)> results = [];
            foreach (WebsiteMetaObject obj in MetaSiteCore.AllObjects)
            {
                int quality = obj.ObjectGeneric.SearchHelper.GetMatchQuality(id);
                if (quality > 0)
                {
                    results.Add((quality, obj));
                }
            }
            results = [.. results.OrderBy(pair => -pair.Item1).ThenBy(pair => pair.Item2.ObjectGeneric.Type.Name).ThenBy(pair => pair.Item2.ObjectGeneric.Name)];
            StringBuilder outText = new();
            outText.Append("<center>");
            if (results.Count > 0)
            {
                int lastQuality = 0;
                string lastType = "";
                foreach ((int quality, WebsiteMetaObject obj) in results)
                {
                    if (quality != lastQuality)
                    {
                        lastQuality = quality;
                        outText.Append($"<br><h4>{MetaObject.SearchableHelpers.SearchQualityName[quality]} Results</h4><br>");
                    }
                    if (obj.ObjectGeneric.Type.Name != lastType)
                    {
                        lastType = obj.ObjectGeneric.Type.Name;
                        outText.Append($"<br><h4>{lastType}</h4><br>");
                    }
                    outText.Append(obj.HtmlContent);
                }
            }
            outText.Append("</center>");
            DocViewModel model = new()
            {
                IsAll = false,
                CurrentlyShown = results.Count,
                Max = MetaSiteCore.AllObjects.Count,
                Content = new HtmlString(outText.ToString()),
                SearchText = id
            };
            return View(model);
        }
    }
}
