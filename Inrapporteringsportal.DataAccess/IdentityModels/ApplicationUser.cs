using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InrapporteringsPortal.DataAccess.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public int OrganisationsId { get; set; }
        //public string Namn { get; set; }
        //public DateTime? SkapadDatum { get; set; }
        //public string SkapadAv { get; set; }
        //public DateTime? AndradDatum { get; set; }
        //public string AndradAv { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}