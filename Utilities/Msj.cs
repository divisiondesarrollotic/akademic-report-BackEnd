namespace AkademicReport.Utilities
{
    public class Msj
    {
        public static string MsjResetPassword { get; set; } = "Se ha reseteado su contraseña satisfactoria.";
        public static string MsjUsuarioExiste { get; set; } = "Usuario existente";
        public static string MsjUsuarioInsertado { get; set; } = "Usuario Insertado Correctamente";
        public static string MsjInsert { get; set; } = "Se inerto un registro correctamente.";
        public static string MsjDelete { get; set; } = "Se elimino un registro correctamente.";
        public static string MsjNoRegistros { get; set; } = "Se se encontro ningun registro.";
        public static string MsjError { get; set; } = "Se ha producido un error";
        public static string MsjCredencialesIncorrectas { get; set; } = "Su correo o su contrasña son incorrectas, por favor intentelo de nuevo.";
        public static string MsjNoData { get; set; } = "No se ha encontrado ningun registro.";
        public static string MsjUpdate { get; set; } = "Se actualizo un registro de manera satisfactoria.";
        public static string MsjSucces { get; set; } = "se ha realizado una operación con exito.";
        public static string MsjHorarioIncorrecto { get; set; } = "El calculo de las horas en el horario no puede dar decimal.";
        public static string MsjPasoDeCredito { get; set; } = "La carga no puede exceder de los 40 creditos.";
        public static string MsjPasoDeCreditoMedioTimepo { get; set; } = "La carga no puede exceder de los 32 creditos, si el docente es de medio tiempo.";
        public static string MsjNoEliminarCodigo { get; set; } = "No se puede eliminar este concepto, porque hay códigos que dependen de él.";
        public static string MsjNoEliminarPeriodo { get; set; } = "No se puede eliminar este periodo, porque hay una carga que dependen de él.";


    }
}
