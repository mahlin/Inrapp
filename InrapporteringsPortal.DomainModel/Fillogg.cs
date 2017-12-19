using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    [Table("Fillogg")]
    public class Fillogg
    {
        public int Id { get; set; }
        public int LeveransId { get; set; }
        public string Filnamn { get; set; }
        public DateTime Datum { get; set; }
        public int Status { get; set; }
    }
}