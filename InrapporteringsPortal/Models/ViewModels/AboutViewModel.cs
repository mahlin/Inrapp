using System.Collections.Generic;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.Web.Models.ViewModels
{
    public class AboutViewModel
    {
        public IEnumerable<AdmFAQKategori> FaqCategories { get; set; }

    }
}