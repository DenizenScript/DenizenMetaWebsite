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
            string fullNameText;
            if (Object.HasMultipleNames)
            {
                fullNameText = $"<span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span>\n<br>" + string.Join("\n<br>", Object.MultiNames.Skip(1).Select(s => Util.EscapeForHTML(s)));
            }
            else
            {
                fullNameText = $"<span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span>";
            }
            HtmlContent += TableLine("primary", "Action Lines", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\">{fullNameText}</a>", false);
            HtmlContent += TableLine("default", "Triggers", Object.Triggers, true);
            HtmlContent += TableLine("default", "Contexts", WebsiteMetaCommand.HtmlizeTags(Object.Context, Object.Meta), false);
            HtmlContent += TableLine("default", "Determine", string.Join("\n", Object.Determinations), true);
            AddHtmlEndParts();
        }

        public override string GroupingString => "NPC Actions";
    }
}
