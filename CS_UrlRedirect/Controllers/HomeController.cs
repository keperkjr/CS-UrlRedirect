using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;
using CS_UrlRedirect.Services;
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
        private readonly IRedirectService _redirectService;

        public HomeController(ILogger<HomeController> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        private async Task<IActionResult> ShowIndex(int? id = null)
        {
            var model = new IndexViewModel
            {
                redirects = await _context.Redirects.ToListAsync(),
                redirect = new RedirectViewModel()
            };
            if (id.HasValue)
            {
                var redirect = await _redirectService.GetAsync(id.Value);
                var redirectVM = new RedirectViewModel(RedirectViewModel.Action.Update);
                redirect.CopyPropsTo(ref redirectVM);
                model.redirect = redirectVM;
            }
            return View(nameof(Index), model);
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            return await ShowIndex();
        }

        // GET: /mpn
        [HttpGet("{code}")]
        public async Task<IActionResult> DoRedirect(string code)
        {
            var redirect = await _redirectService.MarkAsVisitedAsync(code);
            if (redirect == null)
            {
                return NotFound();
            }
            return new RedirectResult(redirect.Url, false);
        }

        // GET: /Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!await _redirectService.ExistsAsync(id))
            {
                return NotFound();
            }
            return await ShowIndex(id);
        }

        // POST: /Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShortCode,Url,action")] RedirectViewModel redirectVM)
        {
            if (string.IsNullOrWhiteSpace(redirectVM.ShortCode))
            {
                ModelState.AddModelError(nameof(redirectVM.ShortCode), "A short code is required");
            } 
            else
            {
                redirectVM.ShortCode = redirectVM.ShortCode.Trim();
                if (redirectVM.action == RedirectViewModel.Action.Create && await _redirectService.ExistsAsync(redirectVM.ShortCode))
                {
                    ModelState.AddModelError(nameof(redirectVM.ShortCode), "The following short code is unavailable and cannot be used");
                }
            }

            if (string.IsNullOrWhiteSpace(redirectVM.Url))
            {
                ModelState.AddModelError(nameof(redirectVM.Url), "A redirect url is required");
            } 
            else
            {
                redirectVM.Url = redirectVM.Url.Trim();
                if (!Http.IsValidURL(redirectVM.Url))
                {
                    ModelState.AddModelError(nameof(redirectVM.Url), "A valid destination url is required");
                }
            }

            if (ModelState.IsValid)
            {
                switch (redirectVM.action)
                {
                    case RedirectViewModel.Action.Create:
                        await CreateEntry(redirectVM);
                        break;
                    case RedirectViewModel.Action.Update:
                        try
                        {
                            await UpdateEntry(redirectVM);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (await _redirectService.ExistsAsync(redirectVM.Id))
                            {
                                throw;
                            }
                            return NotFound();
                        }
                        break;
                }
                //return RedirectToAction(nameof(Index));
                return Json(new { redirectTo = Url.Action(nameof(Index)) });
            } 
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();

                Debug.Print("");
            }
            return PartialView("_RedirectForm", redirectVM);
        }

        public async Task CreateEntry(RedirectViewModel redirectVM)
        {
            var redirect = new Redirect();
            redirectVM.CopyPropsTo(ref redirect);
            await _redirectService.AddAsync(redirect);
        }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
        public async Task UpdateEntry(RedirectViewModel redirectVM)
        {
            await _redirectService.UpdateAsync(redirectVM.Id, redirectVM);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _redirectService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /privacy
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}