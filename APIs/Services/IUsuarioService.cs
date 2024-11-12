using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;

namespace APIs.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario> ObtenerUsuarioPorIdAsync(long id);
        Task<Usuario> AgregarUsuarioAsync(UsuarioRequest usuarioRequest);
        Task<Usuario> ActualizarUsuarioAsync(UsuarioRequest usuarioRequest);
        Task<bool> EliminarUsuarioAsync(long id);
    }

}
