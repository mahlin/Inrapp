﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InrapporteringsPortal.DomainModel
{
    public class Organisationsenhet
    {
        public int Id { get; set; }
        public int OrganisationsId { get; set; }
        public string Enhetsnamn { get; set; }
        public string Enhetskod{ get; set; }
        public DateTime? AktivFrom { get; set; }
        public DateTime? AktivTom { get; set; }
        public DateTime SkapadDatum { get; set; }
        public string SkapadAv { get; set; }
        public DateTime AndradDatum { get; set; }
        public string AndradAv { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<Leverans> Leveranser { get; set; }
        }
}