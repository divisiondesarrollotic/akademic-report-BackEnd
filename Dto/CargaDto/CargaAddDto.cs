﻿using AkademicReport.Dto.AsignaturaDto;
using System.ComponentModel.DataAnnotations;

namespace AkademicReport.Dto.CargaDto
{
    public class CargaAddDto
    {
        public string periodo { get; set; } = null!;
        public int recinto { get; set; } 
        public string cod_asignatura { get; set; } = null!;
        [Required]
        public int? IdCodigo { get; set; }
        public string nombre_asignatura { get; set; } = null!;
        public string cod_universitas { get; set; } = null!;
        public int seccion { get; set; } 
        public string aula { get; set; } = null!;
        public int? idModalidad { get; set; }
        public int dia_id { get; set; }
        public string hora_inicio { get; set; } = null!;
        public string minuto_inicio { get; set; } = null!;
        public string hora_fin { get; set; } = null!;
        public string minuto_fin { get; set; } = null!;
        public double numero_hora { get; set; }
        public int credito { get; set; }
        public string nombre_profesor { get; set; } = null!;
        public string Cedula { get; set; } = null!;
     
        public int? idTipoCarga { get; set; }

        public bool? HoraContratada { get; set; } = true;
        public int? idUsuario { get; set; }
        // Nuevos campos para manejar las cargas irregulares
        public int? CantSemanas { get; set; }
        public int? IdTipoReporte { get; set; }
        public int? IdTipoReporteIrregular { get; set; }
        public List<DayCantSemanasDto>?CantSemanaMes { get; set; } =  new List<DayCantSemanasDto>();
       
    }
}
