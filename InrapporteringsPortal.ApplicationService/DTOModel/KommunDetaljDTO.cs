using InrapporteringsPortal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.ApplicationService.DTOModel
{
    public class KommunDetaljDTO
    {
        public int Id { get; set; }
        public string Namn { get; set; }

        public string KommunKod { get; set; }

        //TODO
        //Mappar från Domain-obj till DTO. Behövs ej om lika?
        internal static KommunDetaljDTO FromKommun(Kommun kommun)
        {
            if (kommun == null)
                return null;

            return new KommunDetaljDTO
            {
                Id = kommun.Id,
                Namn = kommun.Namn,
                KommunKod = kommun.KommunKod
            };

        }
    }
}