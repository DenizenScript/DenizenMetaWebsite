using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.Models
{
    public class DocViewModel
    {
        public bool IsAll;

        public int CurrentlyShown;

        public int Max;

        public HtmlString Content;

        public string SearchText;
    }
}
