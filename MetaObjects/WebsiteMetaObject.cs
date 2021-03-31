using SharpDenizenTools.MetaObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string ParseAndEscape(string content)
        {
#warning TODO Write the actual parser bit
            return Util.EscapeForHTML(content).Replace("\n", "\n<br>");
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

        public static string URLSafe(string input)
        {
            return input.Replace("<", "%3C").Replace(">", "%3E").Replace("\"", "%22");
        }

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
