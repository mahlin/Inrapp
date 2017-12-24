using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    [Table("Levereradfil")]
    public class LevereradFil
    {
        public int Id { get; set; }
        public int LeveransId { get; set; }
        public string Filnamn { get; set; }
        public string NyttFilnamn { get; set; }
        public int Ordningsnr { get; set; }
        public string Filstatus { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
    }
}