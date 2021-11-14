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
            HtmlContent += TableLine("primary", "Event Lines", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\">{string.Join("\n<br>", Object.Events.Select(s => Util.EscapeForHTML(s.Replace("'", ""))))}</a>", false);
            HtmlContent += TableLine("dark", "Generated Examples", string.Join('\n', Object.Events.Select(GenerateSample).JoinWith(Object.Events.Select(GenerateSample)).Distinct()), true);
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
