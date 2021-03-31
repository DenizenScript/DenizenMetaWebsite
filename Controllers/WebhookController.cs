using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenizenMetaWebsite.Controllers
{
    public class WebhookController : Controller
    {
        public IActionResult Reload()
        {
            if (Request.Method != "POST")
            {
                return BadRequest();
            }
            if (string.IsNullOrWhiteSpace(MetaSiteCore.ReloadWebhookToken))
            {
                return NotFound();
            }
            if (!Request.Query.ContainsKey("token"))
            {
                return BadRequest();
            }
            if (Request.Query["token"].First() != MetaSiteCore.ReloadWebhookToken)
            {
                return Forbid();
            }
            MetaSiteCore.ReloadMeta();
            return Ok();
        }
    }
}
