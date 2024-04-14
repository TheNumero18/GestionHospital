namespace GestionHospital.Models.DTOs
{
    public class DiagnosticoPacienteDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Diagnostico { get; set; }
        public string NombrePaciente { get; set; }
        public string NombreMedico { get; set; }
    }
}
