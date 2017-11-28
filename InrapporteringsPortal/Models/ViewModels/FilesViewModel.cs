﻿
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
        public int? SelectedRegisterId { get; set; }
        public List<KeyValuePair<int, string>> RegisterInfoText { get; set; }
    }
}