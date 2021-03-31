using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaTag : WebsiteMetaObject<MetaTag>
    {
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.TagFull)}</span></a>", false);
            HtmlContent += TableLine("active", "Returns", Object.Returns, true);
            if (!string.IsNullOrWhiteSpace(Object.Mechanism))
            {
                HtmlContent += TableLine("active", "Related Mechanism", $"<a href=\"/Docs/Mechanisms/{URLSafe(Object.Mechanism)}\">{Util.EscapeForHTML(Object.Mechanism)}</a>", false);
            }
            HtmlContent += TableLine("active", "Description", Object.Description, true);
            AddHtmlEndParts();
        }

        public override bool MatchesSearch(string search)
        {
            return Object.CleanName.Contains(search);
        }

        public override string GroupingString => Object.BeforeDot;
    }
}
