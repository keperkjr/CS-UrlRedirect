using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;

namespace CS_UrlRedirect.Controllers
{
    [Route("r")]
    public class RedirectsController : Controller
    {
        private readonly DatabaseDBContext _context;

        public RedirectsController(DatabaseDBContext context)
        {
            _context = context;
        }

        // GET: Redirects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Redirects.ToListAsync());
        }

        // GET: Redirects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirect = await _context.Redirects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (redirect == null)
            {
                return NotFound();
            }

            return View(redirect);
        }

        // GET: Redirects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Redirects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShortCode,Url,NumVisits")] Redirect redirect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(redirect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(redirect);
        }

        // GET: Redirects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirect = await _context.Redirects.FindAsync(id);
            if (redirect == null)
            {
                return NotFound();
            }
            return View(redirect);
        }

        // POST: Redirects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShortCode,Url,NumVisits")] Redirect redirect)
        {
            if (id != redirect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(redirect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedirectExists(redirect.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(redirect);
        }

        // GET: Redirects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redirect = await _context.Redirects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (redirect == null)
            {
                return NotFound();
            }

            return View(redirect);
        }

        // POST: Redirects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redirect = await _context.Redirects.FindAsync(id);
            _context.Redirects.Remove(redirect);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedirectExists(int id)
        {
            return _context.Redirects.Any(e => e.Id == id);
        }
    }
}
