using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class AdmFilkrav
    {
        public int Id { get; set; }
        public int DelregisterId { get; set; }
        public int? ForeskriftsId { get; set; }
        public string Namn { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual AdmDelregister AdmDelregister { get; set; }
        public virtual AdmForeskrift AdmForeskrift { get; set; }
        public virtual ICollection<AdmForvantadfil> AdmForvantadfil { get; set; }
        public virtual ICollection<AdmForvantadleverans> AdmForvantadleverans { get; set; }

    }
}