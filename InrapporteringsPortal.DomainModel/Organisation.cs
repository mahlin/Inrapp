using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Organisation
    {
        //[Column("organisationsid")]
        public int Id { get; set; }
        public string Landstingskod { get; set; }
        public string Kommunkod { get; set; }
        public string Inrapporteringskod { get; set; }
        public string Organisationstyp { get; set; }
        public string Organisationsnr { get; set; }
        public string Organisationsnamn { get; set; }
        public string Hemsida { get; set; }
        public string EpostAdress { get; set; }
        public string Telefonnr { get; set; }
        public string Adress { get; set; }
        public string Postnr { get; set; }
        public string Postort { get; set; }
        public string Epostdoman { get; set; }
        public DateTime? AktivFrom { get; set; }
        public DateTime? AktivTom { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Leverans> Leveranser { get; set; }
        public virtual ICollection<AdmUppgiftsskyldighet> AdmUppgiftsskyldighet { get; set; }
        public virtual ICollection<Organisationsenhet> Organisationsenhet { get; set; }
    }
}