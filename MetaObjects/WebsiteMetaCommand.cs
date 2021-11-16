using DenizenMetaWebsite.Highlighters;
using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaHandlers;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public class WebsiteMetaCommand : WebsiteMetaObject<MetaCommand>
    {
        public static string HtmlizeSyntax(string syntax)
        {
            string cmd = syntax.BeforeAndAfter(' ', out syntax);
            StringBuilder output = new(syntax.Length * 2);
            output.Append($"<span class=\"syntax_command\">{Util.EscapeForHTML(cmd)}</span> ");
            int spans = 0;
            foreach (char c in syntax)
            {
                switch (c)
                {
                    case '<':
                        spans++;
                        output.Append("<span class=\"syntax_fillable\">&lt;");
                        break;
                    case '>':
                        spans--;
                        output.Append("&gt;</span>");
                        break;
                    case '{':
                        spans++;
                        output.Append("<span class=\"syntax_default\">{");
                        break;
                    case '}':
                        spans--;
                        output.Append("}</span>");
                        break;
                    case '[':
                        spans++;
                        output.Append("<span class=\"syntax_required\">[");
                        break;
                    case ']':
                        spans--;
                        output.Append("]</span>");
                        break;
                    case '(':
                        spans++;
                        output.Append("<span class=\"syntax_optional\">(");
                        break;
                    case ')':
                        spans--;
                        output.Append(")</span>");
                        break;
                    case '&':
                        output.Append("&amp;");
                        break;
                    case ':':
                        output.Append("<span class=\"syntax_colon\">:</span>");
                        break;
                    case '.':
                    case '|':
                    case '/':
                        output.Append($"<span class=\"syntax_list\">{c}</span>");
                        break;
                    default:
                        output.Append(c);
                        break;
                }
            }
            for (int i = 0; i < spans; i++)
            {
                output.Append("</span>");
            }
            if (spans < 0)
            {
                Console.Error.WriteLine($"Span misalign for command {cmd}: got {output} which is {spans}");
                return $"<b>(ERROR: SPAN MISALIGN {spans})</b>";
            }
            return $"<code>{output}</code>";
        }

        public static string HtmlizeTags(string[] tags, MetaDocs meta)
        {
            StringBuilder tagOutput = new();
            foreach (string tag in tags)
            {
                string[] parts = tag.Split(' ', 2);
                string properName = $"<code>{ScriptHighlighter.ColorArgument(Util.EscapeForHTML(parts[0]), false)}</code>";
                if (parts.Length == 2)
                {
                    tagOutput.Append(properName).Append(' ').Append(ParseAndEscape(parts[1]));
                }
                else
                {
                    MetaTag actualTag = meta.FindTag(parts[0]);
                    if (actualTag == null)
                    {
                        string nameLow = parts[0].ToLowerFast();
                        tagOutput.Append(Util.EscapeForHTML(parts[0])).Append((nameLow == "none" || nameLow == "todo") ? "" : " ERROR: TAG INVALID");
                    }
                    else
                    {
                        string desc = ParseAndEscape(actualTag.Description);
                        if (desc.Contains('\n'))
                        {
                            desc = desc.Before('\n') + $" <a href=\"/Docs/Tags/{actualTag.CleanedName}\">(...)</a>";
                        }
                        tagOutput.Append($"<a href=\"/Docs/Tags/{actualTag.CleanedName}\">").Append(properName).Append("</a> ").Append(desc);
                    }
                }
                tagOutput.Append("\n<br>");
            }
            return tagOutput.ToString();
        }

        public override void LoadHTML()
        {
            HtmlContent = HTML_PREFIX;
            string aID = Util.EscapeForHTML(Object.CleanName);
            HtmlContent += TableLine("primary", "Name", $"<a id=\"{aID}\" href=\"#{aID}\" onclick=\"doFlashFor('{aID}')\"><span class=\"doc_name\">{Util.EscapeForHTML(Object.Name)}</span></a>", false);
            HtmlContent += TableLine("active", "Syntax", HtmlizeSyntax(Object.Syntax), false);
            HtmlContent += TableLine("active", "Short Description", Object.Short, true);
            HtmlContent += TableLine("active", "Full Description", Object.Description, true);
            HtmlContent += TableLine("active", "Related Tags", HtmlizeTags(Object.Tags, Object.Meta), false);
            foreach (string usage in Object.Usages)
            {
                HtmlContent += TableLine("active", "Usage Example", ScriptHighlighter.Highlight("#" + usage), false);
            }
            AddHtmlEndParts();
        }

        public override string GroupingString => Object.Group;
    }
}
