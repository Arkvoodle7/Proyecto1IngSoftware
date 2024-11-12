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

        public async Task<OrdenCompra> ActualizarOCAsync(OrdenCompra oc)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var query = "UPDATE OCS SET FECHA = :FECHA, TOTAL = :TOTAL, APLICADA = :APLICADA, ID_PROV = :ID_PROV WHERE NUM_OC = :NUM_OC";

            try
            {
                using var command = new OracleCommand(query, connection);
                command.Parameters.Add("NUM_OC", OracleDbType.Int32).Value = oc.NumOC;
                command.Parameters.Add("FECHA", OracleDbType.Date).Value = oc.Fecha;
                command.Parameters.Add("TOTAL", OracleDbType.Decimal).Value = oc.Total;
                command.Parameters.Add("APLICADA", OracleDbType.Varchar2).Value = oc.Aplicada;
                command.Parameters.Add("ID_PROV", OracleDbType.Int32).Value = oc.IdProv;
                await command.ExecuteNonQueryAsync();

                return oc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<OrdenCompra> AgregarOCAsync(OrdenCompra oc)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            try
            {
                using var command = new OracleCommand("INSERT INTO OCS (FECHA, TOTAL, APLICADA, ID_PROV) VALUES (:FECHA, :TOTAL, :APLICADA, :ID_PROV)", connection);
                command.Parameters.Add("FECHA", OracleDbType.Date).Value = oc.Fecha;
                command.Parameters.Add("TOTAL", OracleDbType.Decimal).Value = oc.Total;
                command.Parameters.Add("APLICADA", OracleDbType.Varchar2).Value = oc.Aplicada;
                command.Parameters.Add("ID_PROV", OracleDbType.Int32).Value = oc.IdProv;
                await command.ExecuteNonQueryAsync();

                return oc;
            }
            catch (Exception ex)
            {
                throw new Exception((ex.ToString()));
            }

        }

        public async Task<OrdenCompra> EliminarOCAsync(int Num_OC)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();
            var query = "DELETE FROM OCS WHERE NUM_OC = :NUM_OC";

            try
            {
                using var command = new OracleCommand(query, connection);
                command.Parameters.Add("NUM_OC", OracleDbType.Int32).Value = Num_OC;
                int filasAfectadas = await command.ExecuteNonQueryAsync();

                if (filasAfectadas > 0)
                {
                    return new OrdenCompra();
                }
                else {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new Exception((ex.ToString()));
            };
        }

        public async Task<List<OrdenCompra>> ObtenerOCAsync(int Num_OC)
        {
            var orden = new List<OrdenCompra>();

            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT * FROM OCS WHERE NUM_OC = :NUM_OC", connection);
            command.Parameters.Add("NUM_OC", OracleDbType.Int32).Value = Num_OC;
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orden.Add(new OrdenCompra
                {
                    NumOC = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    Total = reader.GetDecimal(2),
                    Aplicada = reader.GetString(3),
                    IdProv = reader.GetInt32(4)
                });
            }

            return orden;
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