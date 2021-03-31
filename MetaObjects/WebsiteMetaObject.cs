using DenizenMetaWebsite.Highlighters;
using FreneticUtilities.FreneticExtensions;
using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.MetaObjects
{
    public abstract class WebsiteMetaObject
    {
        /// <summary>The HTML-displayable content for this object.</summary>
        public string HtmlContent;

        /// <summary>Load the object's data to fill the <see cref="HtmlContent"/>.</summary>
        public abstract void LoadHTML();

        /// <summary>Return true if the object matches some search text, otherwise return false.</summary>
        public abstract bool MatchesSearch(string search);

        /// <summary>Object group or similar, if any, for sorting.</summary>
        public abstract string GroupingString { get; }

        public const string HTML_PREFIX = "<table class=\"table table-hover\"><tbody>\n";

        public const string HTML_SUFFIX = "</tbody></table>\n";

        public static string EscapeQuickSimple(string content)
        {
            return Util.EscapeForHTML(content).Replace("\n", "\n<br>");
        }

        public static string URLSafe(string input)
        {
            return input.Replace("<", "%3C").Replace(">", "%3E").Replace("\"", "%22");
        }

        public static string ParseLinksHelper(string content)
        {
            int linkStart = content.IndexOf("<@link");
            if (linkStart == -1)
            {
                return EscapeQuickSimple(content);
            }
            int tagMarks = 0;
            int linkEnd = -1;
            for (int i = linkStart + 1; i < content.Length; i++)
            {
                if (content[i] == '<')
                {
                    tagMarks++;
                }
                else if (content[i] == '>')
                {
                    if (tagMarks == 0)
                    {
                        linkEnd = i;
                        break;
                    }
                    tagMarks--;
                }
            }
            if (linkEnd == -1)
            {
                Console.Error.WriteLine("Invalid link text found in " + content);
                return EscapeQuickSimple(content);
            }
            string[] linkContent = content[(linkStart + "<@link ".Length)..linkEnd].Split(' ', 2);
            string escapedName = Util.EscapeForHTML(linkContent[1]);
            string targetName = URLSafe(linkContent[1]);
            string fixedLink;
            switch (linkContent[0].ToLowerFast())
            {
                case "url":
                    if (targetName.StartsWith("https://"))
                    {
                        fixedLink = $"URL:<a href=\"{targetName}\">{escapedName}</a>";
                    }
                    else
                    {
                        Console.Error.WriteLine("Dangerous URL '" + targetName + "' found in " + content);
                        fixedLink = "Link Blocked";
                    }
                    break;
                case "command":
                    fixedLink = $"Command:<a href=\"/Docs/Commands/{targetName}\">{escapedName}</a>";
                    break;
                case "tag":
                    fixedLink = $"Tag:<a href=\"/Docs/Tags/{targetName}\">{escapedName}</a>";
                    break;
                case "event":
                    fixedLink = $"Event:<a href=\"/Events/Commands/{targetName}\">{escapedName}</a>";
                    break;
                case "mechanism":
                    fixedLink = $"Mechanism:<a href=\"/Docs/Mechanisms/{targetName}\">{escapedName}</a>";
                    break;
                case "action":
                    fixedLink = $"Action:<a href=\"/Docs/Actions/{targetName}\">{escapedName}</a>";
                    break;
                case "language":
                    fixedLink = $"Language:<a href=\"/Docs/Languages/{targetName}\">{escapedName}</a>";
                    break;
                default:
                    Console.Error.WriteLine("Invalid link type '" + linkContent[0] + "' found in " + content);
                    fixedLink = "Error Invalid Link";
                    break;
            }
            return EscapeQuickSimple(content[..linkStart]) + fixedLink + ParseLinksHelper(content[(linkEnd + 1)..]);
        }

        public static string ParseAndEscape(string content)
        {
            if (!content.Contains("<code>"))
            {
                return ParseLinksHelper(content);
            }
            int lastEnd = 0;
            StringBuilder parsed = new StringBuilder(content.Length * 2);
            int codeBlockStart = content.IndexOf("<code>");
            while (codeBlockStart != -1)
            {
                int codeBlockEnd = content.IndexOf("</code>", codeBlockStart);
                if (codeBlockEnd == -1)
                {
                    Console.Error.WriteLine("Invalid code block found in " + content);
                    break;
                }
                parsed.Append(ParseLinksHelper(content[lastEnd..codeBlockStart])).Append("\n<br>");
                string codeRaw = content[(codeBlockStart + "<code>".Length)..codeBlockEnd];
                parsed.Append(ScriptHighlighter.Highlight(codeRaw));
                lastEnd = codeBlockEnd + "</code>".Length;
                codeBlockStart = content.IndexOf("<code>", lastEnd);
            }
            return parsed.ToString();
        }

        public static string TableLine(string type, string key, string content, bool cleanContent)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "";
            }
            if (cleanContent)
            {
                content = ParseAndEscape(content);
            }
            return $"<tr class=\"table-{type}\"><td class=\"td-doc-key\">{key}</td><td>{content}</td></tr>\n";
        }
    }


    /// <summary>The version of a meta object designed to fit on a website.</summary>
    public abstract class WebsiteMetaObject<T> : WebsiteMetaObject where T : MetaObject
    {
        /// <summary>The original object.</summary>
        public T Object;

        public void AddHtmlEndParts()
        {
            HtmlContent += TableLine("active", "Group", Object.Group, true);
            HtmlContent += TableLine("warning", "Requires", Object.Plugin, true);
            HtmlContent += TableLine("danger", "Warning(s)", string.Join("\n", Object.Warnings), true);
            HtmlContent += TableLine("secondary", "Source", Object.SourceFile.StartsWith("https://") ? $"<a href=\"{URLSafe(Object.SourceFile)}\">{Util.EscapeForHTML(Object.SourceFile)}</a>" : Object.SourceFile, false);
            HtmlContent += HTML_SUFFIX;
        }
    }
}
