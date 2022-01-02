using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CS_UrlRedirect.Models
{
    public partial class Redirect
    {
        public int Id { get; set; }

        [Display(Name = "Redirect Short Code")]
        [Required(ErrorMessage = "A short code is required")]
        public string ShortCode { get; set; }

        [Display(Name = "Destination Url")]
        [Required(ErrorMessage = "A redirect url is required")]
        public string Url { get; set; }

        [Display(Name = "Total Visits")]
        public int NumVisits { get; set; }
    }
}
