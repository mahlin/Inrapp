﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Leverans
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public string ApplicationUserId { get; set; }
        public int DelregisterId { get; set; }
        public string Period { get; set; }
        public DateTime Leveranstidpunkt { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual ICollection<LevereradFil> LevereradeFiler { get; set; }
    }
}