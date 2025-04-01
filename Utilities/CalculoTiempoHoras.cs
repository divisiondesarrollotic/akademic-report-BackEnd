using AkademicReport.Dto.CargaDto;
using AkademicReport.Dto.ReporteDto;
using AkademicReport.Models;

namespace AkademicReport.Utilities
{
    public class CalculoTiempoHoras
    {
        public  static  decimal Calcular(int HoraInicio, int MinutoInicio, int HoraFin, int MinutoFin)
        {

            string TiempoInicio = $"{HoraInicio}: {MinutoInicio}";
            string TiempoFin = $"{HoraFin}: {MinutoFin}";
            var TiempoResultado = Convert.ToDateTime(TiempoFin) - Convert.ToDateTime(TiempoInicio);
            //var test = ((TiempoResultado.Hours*60) % 50);
            var Minutos = (TiempoResultado.Hours * 60) + ((TiempoResultado.Minutes *60)%50) + TiempoResultado.Minutes;
          
            return Minutos / 50;

        }
        
        public static MontosPosgradosDto DistribucionMontosPosgrado(int mes, decimal monto)
        {
            var montos = new MontosPosgradosDto() { Mes1 = 0, Mes2 = 0, Mes3 = 0, Mes4 = 0 };
            if (mes >= 1 && mes <= 12)
            {
                int indiceMes = (mes - 1) % 4;

                switch (indiceMes)
                {
                    case 0: montos.Mes1 = monto; break;
                    case 1: montos.Mes2 = monto; break;
                    case 2: montos.Mes3 = monto; break;
                    case 3: montos.Mes4 = monto; break;
                }
            }
            return montos;
        }

        //public async static List<MesGetDto>DistribucionCuatrimestre(int idMes, int Cuatrimestre, List<MesGetDto>Meses)
        //{
        //    Meses = Meses.Where(c=>c.)
        //}
    }
}

