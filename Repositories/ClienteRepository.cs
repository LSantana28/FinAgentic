using Oracle.ManagedDataAccess.Client;
using CobrAI.Data;
using CobrAI.Models;

namespace CobrAI.Repositories
{
    public class ClienteRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public ClienteRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Cliente?> BuscarClientePorId(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            var sql = @"
                SELECT
                    ID,
                    NOME,
                    EMAIL
                FROM CLIENTE
                WHERE ID = :id
            ";

            using var command = new OracleCommand(sql, connection);

            command.Parameters.Add(new OracleParameter("id", id));

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Cliente
                {
                    Id = Convert.ToInt32(reader["ID"]),
                    Nome = reader["NOME"].ToString(),
                    Email = reader["EMAIL"].ToString()
                };
            }

            return null;
        }
    }
}