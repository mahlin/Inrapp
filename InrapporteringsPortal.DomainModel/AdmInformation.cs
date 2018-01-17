using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class AdmInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Informationstyp{ get; set; }
        public string Text { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
    }
}