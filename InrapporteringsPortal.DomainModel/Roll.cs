﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Roll
    {
        public int Id { get; set; }
        public int DelregisterId { get; set; }
        public int ApplicationUserId { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
    }
}