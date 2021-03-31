using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaMechanism : WebsiteMetaObject<MetaMechanism>
    {
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.FullName)}</span></a>", false);
            HtmlContent += TableLine("active", "Input", Object.Input, true);
            HtmlContent += TableLine("active", "Related Tags", WebsiteMetaCommand.HtmlizeTags(Object.Tags, Object.Meta), false);
            HtmlContent += TableLine("active", "Description", Object.Description, true);
            AddHtmlEndParts();
        }

        public override bool MatchesSearch(string search)
        {
            return Object.CleanName.Contains(search);
        }

        public override string GroupingString => Object.MechObject + " Mechanisms";
    }
}
