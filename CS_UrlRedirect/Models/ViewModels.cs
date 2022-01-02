using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS_UrlRedirect.Models
{
    public class IndexViewModel
    {
        public IEnumerable<CS_UrlRedirect.Models.Redirect> redirects { get; set; }
        public CS_UrlRedirect.Models.RedirectViewModel redirect { get; set; }
    }

    public class RedirectViewModel : CS_UrlRedirect.Models.Redirect
    {
        public enum Action
        {
            Create,
            Update
        }
        public Action action;

        public RedirectViewModel()
        {
            this.action = Action.Create;
        }

        public RedirectViewModel(Action action) {
            this.action = action;
        }
    }
}
