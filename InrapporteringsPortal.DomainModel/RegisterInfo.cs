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
        public bool Selected { get; set; } = false;
        public string InfoText { get; set; }
        public string Slussmapp { get; set; }
        public bool RapporterarPerEnhet { get; set; } = false;
        public List<KeyValuePair<string, string>> Organisationsenheter { get; set; }
        public List<RegisterFilkrav> Filkrav { get; set; }
        public string SelectedFilkrav { get; set; }
        //public string FilMask { get; set; }
        //public string RegExp { get; set; }
        //public string Period { get; set; }
        //public IEnumerable<string> Perioder { get; set; }
        ////public int ForvantadLevransId { get; set; }
        //public bool RapporterarPerEnhet { get; set; } = false;
        //public bool Obligatorisk { get; set; } = true;
        //public List<KeyValuePair<string, string>> Organisationsenheter { get; set; }
        //public IEnumerable<string> FilMasker { get; set; }
        //public IEnumerable<string> RegExper { get; set; }
    }
}