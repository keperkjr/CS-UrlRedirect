using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS_UrlRedirect.Models
{
    public class IndexViewModel
    {
        public IEnumerable<CS_UrlRedirect.Models.Redirect> redirects { get; set; }
        public CS_UrlRedirect.Models.RedirectViewModel redirect { get; set; }
    }

    public class RedirectViewModel
    {
        public enum Action
        {
            Create,
            Update
        }

        [Display(Name = "Id #")]
        public int Id { get; set; }

        [Display(Name = "Redirect Short Code")]
        [Required(ErrorMessage = "A short code is required")]
        public string ShortCode { get; set; }

        [Display(Name = "Destination Url")]
        [Required(ErrorMessage = "A redirect url is required")]
        public string Url { get; set; }
        public Action action { get; set; }

        public RedirectViewModel()
        {
            this.action = Action.Create;
        }

        public RedirectViewModel(Action action) {
            this.action = action;
        }
    }
}
