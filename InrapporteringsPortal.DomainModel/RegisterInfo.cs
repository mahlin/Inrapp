using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class RegisterInfo
    {
        public int Id { get; set; }
        public string Namn { get; set; }
        public string Kortnamn { get; set; }
        public int AntalFiler { get; set; }
        public string InfoText { get; set; }
        public string Slussmapp { get; set; }
        public string FilMask { get; set; }
        public string RegExp { get; set; }
        public string Period { get; set; }
        public int ForvantadLevransId { get; set; }
        public IEnumerable<string> FilMasker { get; set; }
        public IEnumerable<string> RegExper { get; set; }
    }
}