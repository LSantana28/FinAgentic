using Oracle.ManagedDataAccess.Client;
using CobrAI.Data;

namespace CobrAI.Repositories
{
    public class CobrancaRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public CobrancaRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task RegistrarCobranca(
            int faturaId,
            string mensagem,
            string status)
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.OpenAsync();

            var sql = @"
                INSERT INTO COBRANCA
                    (FATURA_ID, MENSAGEM, STATUS)
                VALUES
                    (:faturaId, :mensagem, :status)
            ";

            using var command = new OracleCommand(sql, connection);

            command.Parameters.Add(new OracleParameter("faturaId", faturaId));
            command.Parameters.Add(new OracleParameter("mensagem", mensagem));
            command.Parameters.Add(new OracleParameter("status", status));

            await command.ExecuteNonQueryAsync();
        }
    }
}