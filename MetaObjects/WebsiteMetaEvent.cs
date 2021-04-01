using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaEvent : WebsiteMetaObject<MetaEvent>
    {
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            string fullNameText;
            if (Object.HasMultipleNames)
            {
                fullNameText = $"<span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span>\n<br>" + string.Join("\n<br>", Object.MultiNames.Skip(1).Select(s => Util.EscapeForHTML(s)));
            }
            else
            {
                fullNameText = $"<span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span>";
            }
            HtmlContent += TableLine("primary", "Event Lines", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\">{fullNameText}</a>", false);
            HtmlContent += TableLine("active", "Triggers", Object.Triggers, true);
            HtmlContent += TableLine("active", "Has Player", Object.Player, true);
            HtmlContent += TableLine("active", "Has NPC", Object.NPC, true);
            HtmlContent += TableLine("active", "Switches", string.Join("\n", Object.Switches), true);
            HtmlContent += TableLine("active", "Contexts", WebsiteMetaCommand.HtmlizeTags(Object.Context, Object.Meta), false);
            HtmlContent += TableLine("active", "Determine", string.Join("\n", Object.Determinations), true);
            if (Object.Cancellable)
            {
                HtmlContent += TableLine("active", "Cancellable", "True - This adds <context.cancelled> and determine 'cancelled' or 'cancelled:false'", true);
            }
            if (Object.HasLocation)
            {
                HtmlContent += TableLine("active", "Has Location", "True - This adds the switches 'in:<area>', 'location_flagged:<flag>', ...", true);
            }
            AddHtmlEndParts();
        }

        public override bool MatchesSearch(string search)
        {
            return Object.CleanEvents.Any(a => a.Contains(search));
        }

        public override string GroupingString => Object.Group ?? "Error: Missing Group";
    }
}
