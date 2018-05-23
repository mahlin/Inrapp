
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.Web.Models.ViewModels
{
    public class HistoryViewModels
    {

        public class HistoryViewModel
        {
            public string OrganisationsNamn { get; set; }
            public List<FilloggDetaljDTO> HistorikLista { get; set; }

        }

    }
}