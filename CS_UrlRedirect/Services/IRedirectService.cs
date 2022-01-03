using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS_UrlRedirect.Models;
using Microsoft.AspNetCore.Identity;

namespace CS_UrlRedirect.Services
{
    public interface IRedirectService
    {
        Task<bool> RedirectExists(int id);
        Task<bool> RedirectExists(string code);
        Task<bool> AddRedirectAsync(Redirect newItem);
        Task<Redirect> VisitRedirectAsync(string code);
    }
}