﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Aterkoppling
    {
        public int Id { get; set; }
        public int LeveransId { get; set; }
        public string Leveransstatus { get; set; }
        public string Resultatfil { get; set; }
        public DateTime Aterkopplingstidpunkt { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
    }
}