using System.Threading.Tasks;
using APIs.Models;

namespace APIs.Services
{
    public interface ILoginService
    {
        Task<Usuario> ValidarLoginAsync(string correo, string contrasena);
    }
}
