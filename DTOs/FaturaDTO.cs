namespace CobrAI.DTOs
{
    public class FaturaDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataVencimento { get; set; }

        public string Status { get; set; }
    }
}