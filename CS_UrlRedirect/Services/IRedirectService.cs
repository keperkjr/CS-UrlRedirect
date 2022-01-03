using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS_UrlRedirect.Models;
using Microsoft.AspNetCore.Identity;

namespace CS_UrlRedirect.Services
{
    public interface IRedirectService
    {
        Task<bool> RedirectExistsAsync(int id);
        Task<bool> RedirectExistsAsync(string code);
        Task<Redirect> GetRedirectAsync(int id);
        Task<Redirect> GetRedirectAsync(string code);

        Task<bool> AddRedirectAsync(Redirect newItem);
        Task<bool> UpdateRedirectAsync(int id, object newItem);
        Task<bool> DeleteRedirectAsync(int id);

        Task<Redirect> VisitRedirectAsync(string code);
    }
}