using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CS_UrlRedirect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseDBContext _context;
        public HomeController(ILogger<HomeController> logger, DatabaseDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // POST: Redirects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,ShortCode,Url,NumVisits")] RedirectViewModel redirect)
        {
            if (string.IsNullOrWhiteSpace(redirect.ShortCode))
            {
                ModelState.AddModelError(nameof(redirect.ShortCode), "A short code is required");
            }
            if (string.IsNullOrWhiteSpace(redirect.Url))
            {
                ModelState.AddModelError(nameof(redirect.Url), "A url is required");
            }

            if (ModelState.IsValid)
            {
                await AddEntry(redirect);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task AddEntry(Redirect redirect)
        {
            _context.Add(redirect);
            await _context.SaveChangesAsync();
        }
    }
}
