using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIs.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace APIs.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            var usuarios = new List<Usuario>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM USUARIOS";
            using var command = new OracleCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                usuarios.Add(new Usuario
                {
                    Id = reader.GetInt64(0),
                    Nombre = reader.GetString(1),
                    Apellido1 = reader.GetString(2),
                    Apellido2 = reader.GetString(3),
                    Telefono = reader.GetInt64(4),
                    Correo = reader.GetString(5),
                    Contrasena = reader.GetString(6),
                    Rol = reader.GetString(7)
                });
            }

            return usuarios;
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(long id)
        {
            Usuario usuario = null;

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM USUARIOS WHERE ID = :ID";
            using var command = new OracleCommand(query, connection);
            command.Parameters.Add("ID", OracleDbType.Int64).Value = id;

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                usuario = new Usuario
                {
                    Id = reader.GetInt64(0),
                    Nombre = reader.GetString(1),
                    Apellido1 = reader.GetString(2),
                    Apellido2 = reader.GetString(3),
                    Telefono = reader.GetInt64(4),
                    Correo = reader.GetString(5),
                    Contrasena = reader.GetString(6),
                    Rol = reader.GetString(7)
                };
            }

            return usuario;
        }

        public async Task<Usuario> AgregarUsuarioAsync(UsuarioRequest usuarioRequest)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "INSERT INTO USUARIOS (NOMBRE, APELLIDO1, APELLIDO2, TELEFONO, CORREO, CONTRASENA, ROL) VALUES (:NOMBRE, :APELLIDO1, :APELLIDO2, :TELEFONO, :CORREO, :CONTRASENA, :ROL) RETURNING ID INTO :ID";
            using var command = new OracleCommand(query, connection);

            command.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = usuarioRequest.Nombre;
            command.Parameters.Add("APELLIDO1", OracleDbType.Varchar2).Value = usuarioRequest.Apellido1;
            command.Parameters.Add("APELLIDO2", OracleDbType.Varchar2).Value = usuarioRequest.Apellido2;
            command.Parameters.Add("TELEFONO", OracleDbType.Decimal).Value = usuarioRequest.Telefono;
            command.Parameters.Add("CORREO", OracleDbType.Varchar2).Value = usuarioRequest.Correo;
            command.Parameters.Add("CONTRASENA", OracleDbType.Varchar2).Value = usuarioRequest.Contrasena;
            command.Parameters.Add("ROL", OracleDbType.Varchar2).Value = usuarioRequest.Rol;

            var idParameter = new OracleParameter("ID", OracleDbType.Decimal) { Direction = ParameterDirection.ReturnValue };
            command.Parameters.Add(idParameter);

            await command.ExecuteNonQueryAsync();

            var usuario = new Usuario
            {
                Id = ((Oracle.ManagedDataAccess.Types.OracleDecimal)idParameter.Value).ToInt64(),
                Nombre = usuarioRequest.Nombre,
                Apellido1 = usuarioRequest.Apellido1,
                Apellido2 = usuarioRequest.Apellido2,
                Telefono = usuarioRequest.Telefono,
                Correo = usuarioRequest.Correo,
                Contrasena = usuarioRequest.Contrasena,
                Rol = usuarioRequest.Rol
            };

            return usuario;
        }

        public async Task<Usuario> ActualizarUsuarioAsync(UsuarioRequest usuarioRequest)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE USUARIOS SET NOMBRE = :NOMBRE, APELLIDO1 = :APELLIDO1, APELLIDO2 = :APELLIDO2, TELEFONO = :TELEFONO, CORREO = :CORREO, CONTRASENA = :CONTRASENA, ROL = :ROL WHERE ID = :ID";
            using var command = new OracleCommand(query, connection);
            command.BindByName = true; // Add this line to bind parameters by name

            // Now add your parameters (order doesn't matter when BindByName is true)
            command.Parameters.Add("ID", OracleDbType.Int64).Value = usuarioRequest.Id;
            command.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = usuarioRequest.Nombre;
            command.Parameters.Add("APELLIDO1", OracleDbType.Varchar2).Value = usuarioRequest.Apellido1;
            command.Parameters.Add("APELLIDO2", OracleDbType.Varchar2).Value = usuarioRequest.Apellido2;
            command.Parameters.Add("TELEFONO", OracleDbType.Int64).Value = usuarioRequest.Telefono;
            command.Parameters.Add("CORREO", OracleDbType.Varchar2).Value = usuarioRequest.Correo;
            command.Parameters.Add("CONTRASENA", OracleDbType.Varchar2).Value = usuarioRequest.Contrasena;
            command.Parameters.Add("ROL", OracleDbType.Varchar2).Value = usuarioRequest.Rol;

            await command.ExecuteNonQueryAsync();

            return new Usuario
            {
                Id = usuarioRequest.Id,
                Nombre = usuarioRequest.Nombre,
                Apellido1 = usuarioRequest.Apellido1,
                Apellido2 = usuarioRequest.Apellido2,
                Telefono = usuarioRequest.Telefono,
                Correo = usuarioRequest.Correo,
                Contrasena = usuarioRequest.Contrasena,
                Rol = usuarioRequest.Rol
            };
        }

        public async Task<bool> EliminarUsuarioAsync(long id)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM USUARIOS WHERE ID = :ID";
            using var command = new OracleCommand(query, connection);
            command.Parameters.Add("ID", OracleDbType.Int64).Value = id;

            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}
