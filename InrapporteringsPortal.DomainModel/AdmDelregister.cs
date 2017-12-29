using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class AdmDelregister
    {
        public int Id { get; set; }
        public int RegisterId { get; set; }
        public string Delregisternamn { get; set; }
        public string Beskrivning { get; set; }
        public string Kortnamn { get; set; }
        public string Slussmapp { get; set; }
        public string Inrapporteringsportal { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual AdmRegister AdmRegister { get; set; }
        public virtual ICollection<AdmFilkrav> AdmFilkrav { get; set; }

    }
}