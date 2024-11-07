using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using APIs.Models;

namespace APIs.Services
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly string _connectionString;

        public OrdenCompraService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<List<OrdenCompra>> ObtenerOrdenesCompraAsync()
        {
            var ordenes = new List<OrdenCompra>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT * FROM OCS", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ordenes.Add(new OrdenCompra
                {
                    NumOC = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    Total = reader.GetDecimal(2),
                    Aplicada = reader.GetString(3),
                    IdProv = reader.GetInt32(4)
                });
            }

            return ordenes;
        }
    }
}