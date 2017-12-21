using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InrapporteringsPortal.DomainModel
{
    [Table("Kommun")]
    public class Kommun
    {
        [Key]
        //[ForeignKey("OrganisationsId")]
        public int OrganisationsId { get; set; }
        public string KommunKod { get; set; }
        public string Lan { get; set; }
        public string Domain { get; set; }

    }
}