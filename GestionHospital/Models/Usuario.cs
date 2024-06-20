namespace GestionHospital.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string[] Roles { get; set; }
    }
}
