using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaEvent : WebsiteMetaObject<MetaEvent>
    {
        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.CleanName)}</span></a>", false);
            HtmlContent += TableLine("secondary", "Event Lines", string.Join("\n<br>", Object.Events.Select(s => GenerateExplainer(s))), false);
            string generatedWarning = "title=\"This example is generated randomly based on the event's format specification. Specific details such as item/entity type names may not actually be applicable to this event.\"";
            string samples = string.Join("\n<br>", Object.Events.Select(GenerateSample).JoinWith(Object.Events.Select(GenerateSample)).Select(Util.EscapeForHTML).Distinct());
            HtmlContent += TableLine("active text-muted smaller_text", $"<abbr {generatedWarning}>Generated Examples</abbr>", $"<span {generatedWarning}>{samples}</span>", false);
            HtmlContent += TableLine("active", "Triggers", Object.Triggers, true);
            if (!string.IsNullOrWhiteSpace(Object.Player))
            {
                HtmlContent += TableLine("active", "Has Player", Object.Player + " - this adds switches 'flagged:<flag name>' + 'permission:<node>', in addition to the '<player>' link.", true);
            }
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

        public override string GroupingString => Object.Group ?? "Error: Missing Group";

        private static readonly Random random = new Random();

        public string GenerateExplainer(string line)
        {
            StringBuilder sb = new StringBuilder(line.Length * 2);
            int spans = 0;
            foreach (string parta in line.SplitFast(' '))
            {
                string part = parta;
                sb.Append(' ');
                if (part.StartsWithFast('('))
                {
                    sb.Append("<span title=\"Optional input: you can exclude this part if you don't need it.\" class=\"syntax_optional\">(");
                    spans++;
                    part = part[1..];
                }
                bool isFillIn = part.StartsWithFast('<');
                if (isFillIn)
                {
                    sb.Append("<span title=\"Fill-in spot: supply a value of the correct type\" class=\"syntax_fillable\">&lt;");
                    spans++;
                    part = part[1..];
                }
                string afterPart = "";
                if (part.EndsWithFast(')'))
                {
                    afterPart = ")</span>";
                    spans--;
                    part = part[..^1];
                }
                if (part.EndsWith(">"))
                {
                    afterPart = "&gt;</span>" + afterPart;
                    spans--;
                    part = part[..^1];
                }
                part = part.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
                if (part.Contains('|'))
                {
                    sb.Append($"<span title=\"List of options: pick one\" class=\"syntax_list\">{part}</span>");
                }
                else if (isFillIn)
                {
                    if (part.Contains('\''))
                    {
                        sb.Append($"<abbr title=\"Fill-in spot for event-specific data\" class=\"syntax_fillable\">{part.Replace("'", "")}</abbr>");
                    }
                    else
                    {
                        sb.Append($"<abbr title=\"Fill-in spot: {GenerateFillInExplainer(part)}\" class=\"syntax_fillable\">{part}</abbr>");
                    }
                }
                else
                {
                    sb.Append(part);
                }
                sb.Append(afterPart);
            }
            for (int i = 0; i < spans; i++)
            {
                sb.Append("</span>");
            }
            if (spans < 0)
            {
                return "<b>(ERROR: SPAN MISALIGN)</b>";
            }
            return $"<code><span class=\"syntax_command\">{sb}</span></code>";
        }

        public string GenerateFillInExplainer(string part)
        {
            switch (part)
            {
                case "entity": return "fill in any valid entity type, entity script name, or other EntityTag matcher";
                case "projectile": return "fill in any valid projectile entity type (like an arrow or snowball), entity script name, or other EntityTag matcher";
                case "vehicle": return "fill in any valid vehicle entity type (like a minecart or a horse), entity script name, or other EntityTag matcher";
                case "item": return "fill in any valid item material type, item script name, or other ItemTag matcher";
                case "block": return "fill in any valid block material type or other MaterialTag matcher";
                case "material": return "fill in any valid MaterialTag matcher";
                case "area": return "fill in any valid area note name, or 'cuboid', 'polygon', 'ellipsoid', or 'area_flagged:', or other area matcher";
                case "inventory": return "fill in any valid inventory type name, inventory script name, inventory note name, or other InventoryTag matcher";
                case "world": return "fill in any world name, or just 'world'";
                default: return "refer to event documentation to learn what you can fill this spot with";
            }
        }

        public string GenerateSample(string line)
        {
            StringBuilder sb = new StringBuilder(line.Length * 2);
            sb.Append(random.NextDouble() > 0.4 ? "after" : "on");
            string[] parts = line.SplitFast(' ');
            bool skipSpace = false;
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (!skipSpace)
                {
                    sb.Append(' ');
                }
                if (part.EndsWithFast(')'))
                {
                    part = part[..^1];
                }
                if (part.StartsWithFast('('))
                {
                    if (random.NextDouble() > 0.5)
                    {
                        skipSpace = true;
                        while (i < parts.Length && !parts[i].EndsWithFast(')'))
                        {
                            i++;
                        }
                        continue;
                    }
                    else
                    {
                        part = part[1..];
                    }
                }
                if (part.StartsWithFast('<') && part.EndsWithFast('>'))
                {
                    sb.Append(Docs.Data.SuggestExampleFor(part[1..^1]));
                }
                else if (part.Contains('|'))
                {
                    string[] options = part.SplitFast('|');
                    sb.Append(options[random.Next(options.Length)]);
                }
                else
                {
                    sb.Append(part);
                }
            }
            sb.Append(':');
            return sb.ToString();
        }
    }
}
