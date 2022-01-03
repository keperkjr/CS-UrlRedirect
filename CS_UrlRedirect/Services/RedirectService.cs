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

    /*
                Task<URLRedirect[]> GetRedirectsForUserAsync(IdentityUser theUser);
                Task<bool> AddRedirectAsync(TodoItem newItem);
                Task<URLRedirect> VisitRedirectAsync(string id);
    */
    public class RedirectService : IRedirectService
    {
        private readonly DatabaseDBContext _context;
        public RedirectService(DatabaseDBContext context)
        {
            _context = context;
        }
        public async Task<bool> RedirectExistsAsync(int Id)
        {
            return await _context.Redirects.AnyAsync(e => e.Id == Id);
        }

        public async Task<bool> RedirectExistsAsync(string shortCode)
        {
            return await _context.Redirects.AnyAsync(e => e.ShortCode == shortCode);
        }

        public async Task<Redirect> GetRedirectAsync(int id)
        {
            return await _context.Redirects.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Redirect> GetRedirectAsync(string code)
        {
            return await _context.Redirects.FirstOrDefaultAsync(e => e.ShortCode == code);
        }

        public async Task<bool> AddRedirectAsync(Redirect newItem)
        {
            _context.Redirects.Add(newItem);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }        
        public async Task<bool> UpdateRedirectAsync(int id, object updateItem)
        {
            var redirect = await GetRedirectAsync(id);
            _context.Entry(redirect).CurrentValues.SetValues(updateItem);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DeleteRedirectAsync(int id)
        {
            var redirect = await GetRedirectAsync(id);
            _context.Redirects.Remove(redirect);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Redirect> VisitRedirectAsync(string code)
        {
            var item = await GetRedirectAsync(code);
            if (item == null) return null;
            item.NumVisits++;
            var saveResult = await _context.SaveChangesAsync();
            return item;
        }
    }
}
