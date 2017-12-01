using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class AspNetUsers
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberConfirmed { get; set; }
        public int TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public int LockoutEnabled { get; set; }
        public string UserName { get; set; }
        public string KommunKod { get; set; }
    }
}