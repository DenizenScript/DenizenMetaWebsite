using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenizenMetaWebsite.Highlighters;
using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaObjects;

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
            foreach (string example in Object.Examples)
            {
                HtmlContent += TableLine("default", "Example", ScriptHighlighter.Highlight(example), false);
            }
            if (Object.Examples.IsEmpty())
            {
                string example = GenerateExample();
                if (example is not null)
                {
                    string generatedWarning = "title=\"This example is generated randomly based on the tag's format specification. Specific details such as item/entity type names may not actually be applicable to this tag.\"";
                    HtmlContent += TableLine("default text-muted slightly_smaller_text", $"<abbr {generatedWarning}>Generated Example</abbr>", $"<span {generatedWarning}>{ScriptHighlighter.Highlight(example)}</span>", false);
                }
            }
            AddHtmlEndParts();
        }

        public string GenerateExample()
        {
            if (!Object.Meta.ObjectTypes.TryGetValue(Object.MechObject.ToLowerFast(), out MetaObjectType baseObjType))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(baseObjType.GeneratedExampleAdjust))
            {
                return null;
            }
            string input = Object.Input.ToLowerFast();
            if (input == "none")
            {
                return $"- adjust {baseObjType.GeneratedExampleAdjust} {Object.MechName}";
            }
            string[] paramSet = input switch { "elementtag(boolean)" => new[] { "true", "false" }, "elementtag(number)" => WebsiteMetaTag.SampleIntegers, "elementtag(decimal)" => WebsiteMetaTag.SampleDecimals, _ => null };
            if (paramSet is null)
            {
                if (!Object.Meta.ObjectTypes.TryGetValue(input, out MetaObjectType inputObjType) && !Object.Meta.ObjectTypes.TryGetValue(input + "tag", out inputObjType))
                {
                    return null;
                }
                paramSet = inputObjType.ExampleValues;
                if (paramSet is null)
                {
                    return null;
                }
            }
            if (paramSet.IsEmpty())
            {
                return null;
            }
            return $"- adjust {baseObjType.GeneratedExampleAdjust} {Object.MechName}:{paramSet[Random.Shared.Next(paramSet.Length)]}";
        }

        public override string GroupingString => Object.MechObject + " Mechanisms";
    }
}
