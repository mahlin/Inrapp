using InrapporteringsPortal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.ApplicationService.DTOModel
{
    public class FilloggDetaljDTO
    {
        public int Id { get; set; }
        public int LeveransId { get; set; }
        public string Filnamn { get; set; }
        public string Datum { get; set; }
        public int Status { get; set; }

        internal static FilloggDetaljDTO FromFillogg(LevereradFil fillogg)
        {

            if (fillogg == null)
                return null;

            return new FilloggDetaljDTO
            {
                Id = fillogg.Id,
                LeveransId = fillogg.LeveransId,
                Filnamn = fillogg.Filnamn
            };
        }
    }
}