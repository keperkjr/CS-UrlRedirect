using System;
using System.Collections.Generic;

#nullable disable

namespace CS_UrlRedirect.Models
{
    public partial class Redirect
    {
        public int Id { get; set; }
        public string ShortCode { get; set; }
        public string Url { get; set; }
        public int NumVisits { get; set; }
    }
}
