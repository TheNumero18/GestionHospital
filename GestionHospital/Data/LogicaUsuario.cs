using GestionHospital.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionHospital.Data
{
    public class LogicaUsuario
    {
        public List<Usuario> ListaUsuarios(){
            return new List<Usuario>{
                new Usuario{Nombre = "Admin", Email="Admin@hospital.com", Pass="1234", Roles = new string[] {"Administrador"}},
                new Usuario{Nombre = "Recepcion", Email="Recepcion@hospital.com", Pass="1234", Roles = new string[] {"Recepcion"}},
                new Usuario{Nombre = "Medico", Email="Medico@hospital.com", Pass="1234", Roles = new string[] {"Medico"}}
            };
        }

        public Usuario ValidarUsuario(string email, string pass)
        {
            return ListaUsuarios().Where(lu => lu.Email.ToLower() == email.ToLower() && lu.Pass.ToLower() == pass.ToLower()).FirstOrDefault();
        }
    }
}
