using FreneticUtilities.FreneticToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite
{
    /// <summary>Helper utilities.</summary>
    public static class Util
    {
        public static string HeaderLine = "Denizen Meta Documentation";

        /// <summary>A helper matcher for characters that need HTML escaping.</summary>
        public static AsciiMatcher NeedsEscapeMatcher = new("&<>");

        /// <summary>A helper matcher for characters that need general cleanup.</summary>
        public static AsciiMatcher NeedsCleanupMatcher = new("\0\t\r");

        /// <summary>Escapes some text to be safe to put into HTML.</summary>
        public static string EscapeForHTML(string text)
        {
            if (NeedsCleanupMatcher.ContainsAnyMatch(text))
            {
                text = text.Replace("\0", " ").Replace("\t", "    ").Replace("\r\n", "\n").Replace("\r", "");
            }
            if (NeedsEscapeMatcher.ContainsAnyMatch(text))
            {
                text = text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            }
            return text;
        }
    }
}
