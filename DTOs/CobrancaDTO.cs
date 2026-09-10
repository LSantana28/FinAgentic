namespace CobrAI.DTOs
{
    public class CobrancaDTO
    {
        public int FaturaId { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public int DiasAtraso { get; set; }

        public string Mensagem { get; set; }
    }
}