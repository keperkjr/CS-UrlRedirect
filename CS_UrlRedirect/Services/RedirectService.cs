using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS_UrlRedirect.Data;
using CS_UrlRedirect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CS_UrlRedirect.Services
{
    public class RedirectService : IRedirectService
    {
        private readonly DatabaseDBContext _context;
        public RedirectService(DatabaseDBContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(int Id)
        {
            return await _context.Redirects.AnyAsync(e => e.Id == Id);
        }

        public async Task<bool> ExistsAsync(string shortCode)
        {
            return await _context.Redirects.AnyAsync(e => e.ShortCode == shortCode);
        }

        public async Task<Redirect> GetAsync(int id)
        {
            return await _context.Redirects.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Redirect> GetAsync(string code)
        {
            return await _context.Redirects.FirstOrDefaultAsync(e => e.ShortCode == code);
        }

        public async Task<bool> AddAsync(Redirect newItem)
        {
            _context.Redirects.Add(newItem);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }        
        public async Task<bool> UpdateAsync(int id, object updateItem)
        {
            var redirect = await GetAsync(id);
            _context.Entry(redirect).CurrentValues.SetValues(updateItem);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var redirect = await GetAsync(id);
            _context.Redirects.Remove(redirect);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Redirect> MarkAsVisitedAsync(string code)
        {
            var item = await GetAsync(code);
            if (item == null) return null;
            item.NumVisits++;
            var saveResult = await _context.SaveChangesAsync();
            return item;
        }
    }
}
