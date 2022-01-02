using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Utils;

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

        private async Task<IActionResult> ShowIndex(int? id = null)
        {
            var model = new IndexViewModel
            {
                redirects = await _context.Redirects.ToListAsync()
            };
            if (id.HasValue)
            {
                var redirect = GetRedirect(id.Value);
                var redirectVM = new RedirectViewModel(RedirectViewModel.Action.Update);
                redirect.CopyPropsTo(ref redirectVM);
                model.redirect = redirectVM;
            }
            return View(nameof(Index), model);
        }

        public async Task<IActionResult> Index()
        {
            return await ShowIndex();
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


        public async Task<IActionResult> Edit(int id)
        {
            return await ShowIndex(id);
        }

        // POST: Redirects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, [Bind("Id,ShortCode,Url,NumVisits")] RedirectViewModel redirect)
        {
            if (string.IsNullOrWhiteSpace(redirect.ShortCode))
            {
                ModelState.AddModelError(nameof(redirect.ShortCode), "A short code is required");
            } else
            {
                redirect.ShortCode = redirect.ShortCode.Trim();
                if (RedirectExists(redirect.ShortCode))
                {
                    ModelState.AddModelError(nameof(redirect.ShortCode), "The following short code is unavailable");
                }
            }

            if (string.IsNullOrWhiteSpace(redirect.Url))
            {
                ModelState.AddModelError(nameof(redirect.Url), "A redirect url is required");
            } else
            {
                redirect.Url = redirect.Url.Trim();
            }

            if (ModelState.IsValid)
            {
                switch (redirect.action)
                {
                    case RedirectViewModel.Action.Create:
                        await CreateEntry(redirect);
                        break;
                    case RedirectViewModel.Action.Update:
                        if (id != redirect.Id)
                        {
                            return NotFound();
                        }

                        try
                        {
                            await UpdateEntry(redirect);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (RedirectExists(redirect.Id))
                            {
                                throw;
                            }
                            return NotFound();
                        }
                        break;
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task CreateEntry(Redirect redirect)
        {
            _context.Add(redirect);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntry(Redirect redirect) 
        {
            _context.Update(redirect);
            await _context.SaveChangesAsync();
        }

        private bool RedirectExists(int id)
        {
            return GetRedirect(id) != null;
        }

        private bool RedirectExists(string shortCode)
        {
            return _context.Redirects.Any(e => e.ShortCode == shortCode);
        }

        private Redirect GetRedirect(int id)
        {
            return _context.Redirects.FirstOrDefault(e => e.Id == id);
        }
    }
}
