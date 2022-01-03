using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;
using CS_UrlRedirect.Services;

namespace CS_UrlRedirect.Controllers
{
    [Route("r")]
    public class RedirectsController : Controller
    {
        private readonly IRedirectService _redirectService;

        public RedirectsController(IRedirectService redirectService)
        {
            _redirectService = redirectService;
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> DoRedirect(string code)
        {
            var redirect = await _redirectService.VisitRedirectAsync(code);
            return new RedirectResult(redirect.Url, false);
        }
    }
}
