using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaAction : WebsiteMetaObject<MetaAction>
    {
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            string fullNameText = string.Join("\n<br>", Object.MultiNames.Select(n => Util.EscapeForHTML(n)));
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{fullNameText}</span></a>", false);
            HtmlContent += TableLine("active", "Triggers", Object.Triggers, true);
            HtmlContent += TableLine("active", "Contexts", WebsiteMetaCommand.HtmlizeTags(Object.Context, Object.Meta), false);
            HtmlContent += TableLine("active", "Determine", string.Join("\n", Object.Determinations), true);
            AddHtmlEndParts();
        }

        public override bool MatchesSearch(string search)
        {
            return Object.CleanActions.Any(a => a.Contains(search));
        }

        public override string GroupingString => "NPC Actions";
    }
}
