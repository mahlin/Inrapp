using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    [Table("Organisation")]
    public class Organisation
    {
        [Key]
        public int OrganisationsId { get; set; }
        public string Organisationsnr { get; set; }
        public string Organisationsnamn { get; set; }
        public string Hemsida { get; set; }
        public string Epostadress { get; set; }
        public string Telefonnr { get; set; }
        public string Adress { get; set; }
        public string Postnr { get; set; }
        public string Postort { get; set; }
        public string Epostdoman { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
    }
}