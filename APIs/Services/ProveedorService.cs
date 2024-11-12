using APIs.Models;
using Oracle.ManagedDataAccess.Client;

namespace APIs.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly string _connectionString;
        public ProveedorService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<Proveedor> ActualizarProvAsync(Proveedor prov)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE PROVEEDORES SET NOMBRE = :nombre WHERE ID = :id";

            using var command = new OracleCommand(query, connection);
            command.Parameters.Add("nombre", OracleDbType.Varchar2).Value = prov.Nombre;
            command.Parameters.Add("id", OracleDbType.Int32).Value = prov.Id;

            int filasAfectadas = await command.ExecuteNonQueryAsync();

            if (filasAfectadas > 0)
            {
                return prov;
            }
            else
            {
                throw new Exception("No se encontró el proveedor con el ID especificado.");
            }
        }


        public async Task<Proveedor> AgregarProvAsync(Proveedor prov)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new OracleCommand("INSERT INTO PROVEEDORES (ID, NOMBRE) VALUES (:ID, :NOMBRE)", connection);
                command.Parameters.Add("ID", OracleDbType.Int32).Value = prov.Id;
                command.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = prov.Nombre;
                
                await command.ExecuteNonQueryAsync();

                return prov;
            }
            catch (Exception ex)
            {
                throw new Exception((ex.ToString()));
            }
        }

        public async Task<Proveedor> EliminarProvAsync(int id)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var query = "DELETE FROM PROVEEDORES WHERE ID = :ID";

            try
            {
                using var command = new OracleCommand(query, connection);
                command.Parameters.Add("ID", OracleDbType.Int32).Value = id;
                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    return new Proveedor();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception((ex.ToString()));
            };
        }

        public async Task<List<Proveedor>> ObtenerProveedorAsync(int id)
        {
            var proveedor = new List<Proveedor>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand("SELECT * FROM PROVEEDORES WHERE ID = :ID", connection);
            command.Parameters.Add("ID", OracleDbType.Int32).Value = id;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                proveedor.Add(new Proveedor
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }

            return proveedor;
        }

        public async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            var proveedores = new List<Proveedor>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT * FROM PROVEEDORES", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                proveedores.Add(new Proveedor
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }

            return proveedores;
        }
    }
}
