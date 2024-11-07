using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using APIs.Models;

namespace APIs.Services
{
    public class LoginService : ILoginService
    {
        private readonly string _connectionString;

        public LoginService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<Usuario> ValidarLoginAsync(string correo, string contrasena)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT * FROM USUARIOS WHERE LOWER(CORREO) = LOWER(:correo) AND LOWER(CONTRASENA) = LOWER(:contrasena)", connection);
            command.Parameters.Add(new OracleParameter("correo", correo));
            command.Parameters.Add(new OracleParameter("contrasena", contrasena));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Apellido1 = reader.GetString(2),
                    Apellido2 = reader.GetString(3),
                    Telefono = reader.GetString(4),
                    Correo = reader.GetString(5),
                    Contrasena = reader.GetString(6),
                    Rol = reader.GetString(7)
                };
            }

            return null;
        }
    }
}