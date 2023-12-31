using DenizenMetaWebsite.Highlighters;
using FreneticUtilities.FreneticExtensions;
using FreneticUtilities.FreneticToolkit;
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
            HtmlContent += TableLine("default", "Returns", $"<a href=\"/Docs/ObjectTypes/{Util.EscapeForHTML(Object.Returns.Before('('))}\">{Util.EscapeForHTML(Object.Returns)}</a>", false);
            if (!string.IsNullOrWhiteSpace(Object.Mechanism))
            {
                HtmlContent += TableLine("default", "Mechanism", $"<a href=\"/Docs/Mechanisms/{URLSafe(Object.Mechanism)}\">{Util.EscapeForHTML(Object.Mechanism)}</a>", false);
            }
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

        public static string[] SampleIntegers = ["1", "2", "3", "4"];

        public static string[] SampleDecimals = ["1", "1.5", "2", "-1", "0"];

        public static AsciiMatcher TagParamTextCleaner = new("<>().,");

        public string ExamplifyTag(string tag)
        {
            switch (Object.Returns.ToLowerFast())
            {
                case "elementtag":
                    return $"- narrate {tag}";
                case "objecttag":
                    return $"- narrate \"debug - the object is: {tag}\"";
                case "elementtag(boolean)":
                    return $"- if {tag}:\n    - narrate \"it was true!\"\n- else:\n    - narrate \"it was false!\"";
                case "elementtag(number)":
                    return $"- narrate \"the number value is {tag}\"";
                case "elementtag(decimal)":
                    return $"- narrate \"the decimal value is {tag}\"";
            }
            if (Object.ReturnType is not null && Object.ReturnType.GeneratedReturnUsageExample.Any())
            {
                return Object.ReturnType.GeneratedReturnUsageExample[Random.Shared.Next(Object.ReturnType.GeneratedReturnUsageExample.Count)].Replace("%VALUE%", tag);
            }
            return $"- narrate {tag}";
        }

        public string GenerateExample()
        {
            if (Object.ParsedFormat.Parts.Count > 2)
            {
                return null;
            }
            string baseText;
            if (Object.BaseType is null)
            {
                baseText = Object.ParsedFormat.Parts[0].Text;
            }
            else if (!string.IsNullOrWhiteSpace(Object.BaseType.GeneratedExampleTagBase))
            {
                baseText = Object.BaseType.GeneratedExampleTagBase;
            }
            else
            {
                return null;
            }
            if (!Object.RequiresParam)
            {
                if (Object.ParsedFormat.Parts.Count == 1)
                {
                    return ExamplifyTag($"<{baseText}>");
                }
                return ExamplifyTag($"<{baseText}.{Object.ParsedFormat.Parts[1].Text}>");
            }
            string param;
            string paramLabel = Object.ParsedFormat.Parts.Last().Parameter;
            if (paramLabel == "<#>")
            {
                param = SampleIntegers[Random.Shared.Next(SampleIntegers.Length)];
            }
            else if (paramLabel == "<#.#>")
            {
                param = SampleDecimals[Random.Shared.Next(SampleDecimals.Length)];
            }
            else
            {
                string cleanParam = TagParamTextCleaner.TrimToNonMatches(paramLabel.ToLowerFast());
                if (!Object.Meta.ObjectTypes.TryGetValue(cleanParam, out MetaObjectType inputType) && !Object.Meta.ObjectTypes.TryGetValue(cleanParam + "tag", out inputType))
                {
                    return null;
                }
                if (inputType.ExampleValues.IsEmpty())
                {
                    return null;
                }
                param = inputType.ExampleValues[Random.Shared.Next(inputType.ExampleValues.Length)];
            }
            if (Object.ParsedFormat.Parts.Count == 1)
            {
                return ExamplifyTag($"<{baseText}[{param}]>");
            }
            return ExamplifyTag($"<{baseText}.{Object.ParsedFormat.Parts[1].Text}[{param}]>");
        }

        public override string GroupingString => Object.BeforeDot;
    }
}
