using System.Text;
using System.Text.Json;
using CobrAI.DTOs;

namespace CobrAI.Services
{
    public class IAService
    {
        private readonly IConfiguration _configuration;

        public IAService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GerarMensagemCobranca(CobrancaDTO cobranca)
        {
            var apiKey = _configuration["Gemini:ApiKey"];

            var mensagemPadrao =
                $"Olá {cobranca.NomeCliente}, identificamos uma pendência " +
                $"no valor de R$ {cobranca.Valor:F2}, vencida há " +
                $"{cobranca.DiasAtraso} dias. Solicitamos, por gentileza, " +
                $"a regularização da pendência. Atenciosamente, Equipe CobrAI.";

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return mensagemPadrao;
            }

            try
            {
                var prompt = $"""
            Você é um assistente financeiro responsável por redigir mensagens
            profissionais de cobrança.

            Cliente: {cobranca.NomeCliente}
            Valor em aberto: R$ {cobranca.Valor:F2}
            Data de vencimento: {cobranca.DataVencimento:dd/MM/yyyy}
            Dias em atraso: {cobranca.DiasAtraso}

            Gere uma mensagem curta, educada e profissional.
            Não invente informações.
            Solicite a regularização da pendência.
            Assine como Equipe CobrAI.
            """;

                using var httpClient = new HttpClient();

                var url =
                    "https://generativelanguage.googleapis.com/v1beta/models/" +
                    "gemini-3.6-flash:generateContent";

                httpClient.DefaultRequestHeaders.Add(
                    "x-goog-api-key",
                    apiKey
                );

                var corpo = new
                {
                    contents = new[]
                    {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
                };

                var json = JsonSerializer.Serialize(corpo);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return mensagemPadrao;
                }

                var respostaJson =
                    await response.Content.ReadAsStringAsync();

                using var documento =
                    JsonDocument.Parse(respostaJson);

                return documento.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()
                    ?? mensagemPadrao;
            }
            catch
            {
                return mensagemPadrao;
            }
        }
    }