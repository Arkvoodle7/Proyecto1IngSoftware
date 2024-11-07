using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using APIs.Models;

namespace APIs.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly string _connectionString;

        public FacturaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public async Task<List<Factura>> ObtenerFacturasAsync()
        {
            var facturas = new List<Factura>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT * FROM FACTURAS", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                facturas.Add(new Factura
                {
                    NumFact = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    Total = reader.GetDecimal(2),
                    IdCliente = reader.GetInt32(3)
                });
            }

            return facturas;
        }
    }
}