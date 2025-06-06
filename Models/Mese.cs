﻿using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class Mese
    {
        public Mese()
        {
            CantSemanasMes = new HashSet<CantSemanasMe>();
            CargaDocentes = new HashSet<CargaDocente>();
        }

        public int IdMes { get; set; }
        public string Nombre { get; set; } = null!;
        public int? Cuatrimestre { get; set; }

        public virtual ICollection<CantSemanasMe> CantSemanasMes { get; set; }
        public virtual ICollection<CargaDocente> CargaDocentes { get; set; }
    }
}
