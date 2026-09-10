using CobrAI.Data;
using CobrAI.DTOs;
using Oracle.ManagedDataAccess.Client;

namespace CobrAI.Repositories
{
    public class FaturaRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public FaturaRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<FaturaDTO>> BuscarFaturasVencidas()
        {
            var faturas = new List<FaturaDTO>();

            using var connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            var sql = @"
        SELECT
            ID,
            CLIENTE_ID,
            VALOR,
            DATA_VENCIMENTO,
            STATUS
        FROM FATURA
        WHERE STATUS = 'VENCIDA'
    ";

            using var command = new OracleCommand(sql, connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                faturas.Add(new FaturaDTO
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    ClienteId = Convert.ToInt32(reader["CLIENTE_ID"]),
                    Valor = Convert.ToDecimal(reader["VALOR"]),
                    DataVencimento = Convert.ToDateTime(reader["DATA_VENCIMENTO"]),
                    Status = reader["STATUS"].ToString()
                });
            }

            return faturas;
        }
    }



}