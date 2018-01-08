using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Inloggning
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime Inloggningstidpunkt { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}