
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InrapporteringsPortal.ApplicationService.DTOModel;
using InrapporteringsPortal.ApplicationService.Helpers;
using InrapporteringsPortal.DomainModel;

namespace InrapporteringsPortal.Web.Models.ViewModels
{
    public class FilesViewModel
    {
        public ViewDataUploadFilesResult[] Files { get; set; }
        [Display(Name = "Välj register")]
        public string SelectedRegisterId { get; set; }
        public string SelectedUnitId { get; set; }
        public string SelectedPeriod { get; set; }
        [DisplayName("Inget att rapportera")]
        public bool IngetAttRapportera { get; set; }
        public List<RegisterInfo> RegisterList { get; set; }
        //public List<KeyValuePair<int, string>> RegisterInfoText { get; set; }
        //public List<KeyValuePair<int, string>> FilMask { get; set; }
        //public List<KeyValuePair<int, int>> FilAntal { get; set; }
        public List<FilloggDetaljDTO> HistorikLista { get; set; }
        public string GiltigKommunKod { get; set; }
        public string OrganisationsNamn { get; set; }
        public string StartUrl { get; set; }

    }
}