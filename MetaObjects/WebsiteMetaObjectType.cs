using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaObjectType : WebsiteMetaObject<MetaObjectType>
    {
        public string LinkObjectType(MetaObjectType type) => type is null ? "ERROR: NULL TYPE?" :  $"<a href=\"/Docs/ObjectTypes/{Util.EscapeForHTML(type.CleanName)}\">{Util.EscapeForHTML(type.Name)}</a>";

        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span></a>", false);
            HtmlContent += TableLine("active", "Prefix", Object.Prefix.ToLowerFast() == "none" ? "None" : $"{Object.Prefix}@", true);
            HtmlContent += TableLine("active", "Base Type", Object.BaseType == null ? null : LinkObjectType(Object.BaseType), true);
            HtmlContent += TableLine("active", "Implements", string.Join(", ", Object.Implements.Select(LinkObjectType)), false);
            HtmlContent += TableLine("active", "Identity Format", Object.Format, true);
            HtmlContent += TableLine("active", "Description", Object.Description, true);
            HtmlContent += TableLine("active", "Extended By", string.Join(", ", Object.ExtendedBy.Select(LinkObjectType)), false);
            AddHtmlEndParts();
        }

        public override string GroupingString => Object.Format == "N/A" ? "Pseudo ObjectType" : (Object.Plugin == null ? "Core" : "External");
    }
}
