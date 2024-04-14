using Microsoft.AspNetCore.Identity;

namespace GestionHospital.Models
{
    public class DiagnosticoPaciente
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Diagnostico { get; set; }
        public int PacienteId { get; set; }
        public virtual Paciente? Paciente { get; set; }
        public int MedicoId { get; set; }
        public virtual Medico? Medico { get; set; }
    }
}
