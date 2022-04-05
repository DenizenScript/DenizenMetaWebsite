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
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.MechName)}</span></a>", false);
            HtmlContent += TableLine("default", "Object", $"<a href=\"/Docs/ObjectTypes/{Util.EscapeForHTML(Object.MechObject)}\">{Util.EscapeForHTML(Object.MechObject)}</a>", false);
            HtmlContent += TableLine("default", "Input", Object.Input, true);
            HtmlContent += TableLine("default", "Related Tags", WebsiteMetaCommand.HtmlizeTags(Object.Tags, Object.Meta), false);
            HtmlContent += TableLine("default", "Description", Object.Description, true);
            AddHtmlEndParts();
        }

        public override string GroupingString => Object.MechObject + " Mechanisms";
    }
}
