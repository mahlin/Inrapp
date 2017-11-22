using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.Web.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string RegisterKod { get; set; }
        public int AntalFiler { get; set; }
    }
}