
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InrapporteringsPortal.Web.Helpers;

namespace InrapporteringsPortal.Web.Models.ViewModels
{
    public class FilesViewModel
    {
        public ViewDataUploadFilesResult[] Files { get; set; }
        [Display(Name = "Välj register")]
        public string SelectedRegisterId { get; set; }
        public List<Register> RegisterList { get; set; }
        public List<KeyValuePair<int, string>> RegisterInfoText { get; set; }
        public List<KeyValuePair<int, string>> FilMask { get; set; }
        public List<KeyValuePair<int, int>> FilAntal { get; set; }
        //public List<FilloggDetaljDTO> HistorikLista { get; set; }
        public string GiltigKommunKod { get; set; }

    }
}