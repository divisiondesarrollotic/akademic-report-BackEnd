namespace AkademicReport.Utilities
{
    public class CalculoTiempoHoras
    {
        static public decimal Calcular(int HoraInicio, int MinutoInicio, int HoraFin, int MinutoFin)
        {

            string TiempoInicio = $"{HoraInicio}: {MinutoInicio}";
            string TiempoFin = $"{HoraFin}: {MinutoFin}";
            var TiempoResultado = Convert.ToDateTime(TiempoFin) - Convert.ToDateTime(TiempoInicio);
            //var test = ((TiempoResultado.Hours*60) % 50);
            var Minutos = (TiempoResultado.Hours * 60) + ((TiempoResultado.Minutes *60)%50) + TiempoResultado.Minutes;
          
            return Minutos / 50;

        }
    }
}
