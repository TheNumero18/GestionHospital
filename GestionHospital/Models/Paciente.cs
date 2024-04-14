namespace GestionHospital.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public virtual ICollection<DiagnosticoPaciente>? Diagnostico { get; set; }
    }
}
