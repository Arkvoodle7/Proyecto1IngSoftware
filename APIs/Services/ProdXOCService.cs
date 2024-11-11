using APIs.Models;
using Oracle.ManagedDataAccess.Client;

namespace APIs.Services
{
    public class ProdXOCService : IProdXOCService
    {
        private readonly string _connectionString;
        public ProdXOCService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<ProdXOC> AgregarProdXOCAsync(ProdXOC prodXoc)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new OracleCommand("INSERT INTO PRODUCTOSXOC (OC, PRODUCTO) VALUES (:OC, :PRODUCTO)", connection);
                command.Parameters.Add("OC", OracleDbType.Int32).Value = prodXoc.OC;
                command.Parameters.Add("Producto", OracleDbType.Int32).Value = prodXoc.Producto;

                await command.ExecuteNonQueryAsync();

                return prodXoc;
            }
            catch (Exception ex)
            {
                throw new Exception((ex.ToString()));
            }
        }

        public async Task<ProdXOC> EliminarProdXOCAsync(ProdXOC prodXoc)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var query = "DELETE FROM PRODUCTOSXOC WHERE OC = :OC AND PRODUCTO = :PRODUCTO";

            try
            {
                using var command = new OracleCommand(query, connection);
                command.Parameters.Add("OC", OracleDbType.Int32).Value = prodXoc.OC;
                command.Parameters.Add("Producto", OracleDbType.Int32).Value = prodXoc.Producto;
                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    return new ProdXOC();
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

        public async Task<List<ProdXOC>> ObtenerProdsXOCAsync(int oc)
        {
            var productos = new List<ProdXOC>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new OracleCommand("SELECT * FROM PRODUCTOSXOC WHERE OC = :OC", connection);
            command.Parameters.Add("OC", OracleDbType.Int32).Value = oc;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                productos.Add(new ProdXOC
                {
                    OC = reader.GetInt32(0),
                    Producto = reader.GetInt32(1)
                });
            }

            return productos;
        }
    }
}
