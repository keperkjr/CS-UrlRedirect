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
                redirects = await _context.Redirects.ToListAsync(),
                redirect = new RedirectViewModel()
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
        public async Task<IActionResult> Index([Bind("Id,ShortCode,Url,action")] RedirectViewModel redirectVM)
        {
            if (string.IsNullOrWhiteSpace(redirectVM.ShortCode))
            {
                ModelState.AddModelError(nameof(redirectVM.ShortCode), "A short code is required");
            } 
            else
            {
                redirectVM.ShortCode = redirectVM.ShortCode.Trim();
                if (redirectVM.action == RedirectViewModel.Action.Create && RedirectExists(redirectVM.ShortCode))
                {
                    ModelState.AddModelError(nameof(redirectVM.ShortCode), "The following short code is unavailable");
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
                            if (RedirectExists(redirectVM.Id))
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
            _context.Add(redirect);
            await _context.SaveChangesAsync();
        }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
        public async Task UpdateEntry(RedirectViewModel redirectVM)
        {
            var redirect = GetRedirect(redirectVM.Id);
            //redirectVM.CopyPropsTo(ref redirect);
            _context.Entry(redirect).CurrentValues.SetValues(redirectVM);
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
