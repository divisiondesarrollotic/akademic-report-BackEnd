using System;
using System.Collections.Generic;

namespace AkademicReport.Models
{
    public partial class PlanestudioDocente
    {
        public int Id { get; set; }
        public int IdDocente { get; set; }
        public int IdPlanestudio { get; set; }
    }
}
