using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class AdmForvantadfil
    {
        public int Id { get; set; }
        public int FilkravId { get; set; }
        public string Filmask { get; set; }
        public string Regexp { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual AdmFilkrav AdmFilkrav { get; set; }
    }
}