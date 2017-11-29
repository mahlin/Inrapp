using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Leverans
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int CountyId { get; set; }
    }
}