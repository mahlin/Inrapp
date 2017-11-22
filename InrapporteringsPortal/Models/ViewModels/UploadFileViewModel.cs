using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.Web.Models.ViewModels
{
    public class UploadFileViewModel
    {
        [Display(Name = "Välj register")]
        public int? SelectedRegisterId { get; set; }

    }
}