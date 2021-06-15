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
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span></a>", false);
            HtmlContent += TableLine("active", "Prefix", Object.Prefix.ToLowerFast() == "none" ? "None" : $"{Object.Prefix}@", true);
            HtmlContent += TableLine("active", "Base Type", Object.BaseTypeName, true);
            HtmlContent += TableLine("active", "Implements", string.Join(", ", Object.ImplementsNames.Select(n => $"<a href=\"/Docs/ObjectTypes/{Util.EscapeForHTML(n)}\">{Util.EscapeForHTML(n)}</a>")), false);
            HtmlContent += TableLine("active", "Identity Format", Object.Format, true);
            HtmlContent += TableLine("active", "Description", Object.Description, true);
            IEnumerable<MetaObjectType> extendingTypes = Docs.ObjectTypes.Values.Where(obj => obj.BaseType == Object || obj.Implements.Contains(Object));
            Console.WriteLine($"Found {extendingTypes.Count()} from {Object.Name} out of {Docs.ObjectTypes.Count}");
            HtmlContent += TableLine("active", "Extended By", string.Join(", ", extendingTypes.Select(t => $"<a href=\"/Docs/ObjectTypes/{Util.EscapeForHTML(t.CleanName)}\">{Util.EscapeForHTML(t.Name)}</a>")), false);
            AddHtmlEndParts();
        }

        public override bool MatchesSearch(string search)
        {
            return Object.CleanName.Contains(search);
        }

        public override string GroupingString => Object.Format == "N/A" ? "Pseudo ObjectType" : (Object.Plugin == null ? "Core" : "External");
    }
}
