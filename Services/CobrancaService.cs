using CobrAI.DTOs;
using CobrAI.Repositories;

namespace CobrAI.Services
{
    public class CobrancaService
    {
        private readonly FaturaRepository _faturaRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly IAService _iaService;
        private readonly CobrancaRepository _cobrancaRepository;
        private readonly EmailService _emailService;

        public CobrancaService(
            FaturaRepository faturaRepository,
            ClienteRepository clienteRepository,
            IAService iaService,
            CobrancaRepository cobrancaRepository,
             EmailService emailService)
        {
            _faturaRepository = faturaRepository;
            _clienteRepository = clienteRepository;
            _iaService = iaService;
            _cobrancaRepository = cobrancaRepository;
            _emailService = emailService;
        }
        public async Task<List<FaturaDTO>> BuscarFaturasVencidas()
        {
            var faturas = await _faturaRepository.BuscarFaturasVencidas();

            foreach (var fatura in faturas)
            {
                var cliente = await _clienteRepository.BuscarClientePorId(fatura.ClienteId);

                if (cliente != null)
                {
                    Console.WriteLine($"Cliente: {cliente.Nome}");
                    Console.WriteLine($"E-mail: {cliente.Email}");
                }
            }

            return faturas;
        }
        public async Task<List<CobrancaDTO>> PrepararCobrancas()
        {
            var faturas = await _faturaRepository.BuscarFaturasVencidas();

            var cobrancas = new List<CobrancaDTO>();

            foreach (var fatura in faturas)
            {
                var cliente = await _clienteRepository.BuscarClientePorId(fatura.ClienteId);

                if (cliente != null)
                {
                    var diasAtraso =
                        (DateTime.Today - fatura.DataVencimento.Date).Days;

                    var cobranca = new CobrancaDTO
                    {
                        FaturaId = fatura.Id,
                        NomeCliente = cliente.Nome,
                        EmailCliente = cliente.Email,
                        Valor = fatura.Valor,
                        DataVencimento = fatura.DataVencimento,
                        DiasAtraso = diasAtraso
                    };
                    cobranca.Mensagem = await _iaService.GerarMensagemCobranca(cobranca);
                    cobrancas.Add(cobranca);
                }
            }

            return cobrancas;
        }
        public async Task RegistrarCobrancas()
        {
            var cobrancas = await PrepararCobrancas();

            foreach (var cobranca in cobrancas)
            {
                var emailEnviado = await _emailService.EnviarEmail(
                    cobranca.EmailCliente,
                    cobranca.Mensagem
                );

                var status = emailEnviado ? "ENVIADA" : "ERRO";

                await _cobrancaRepository.RegistrarCobranca(
                    cobranca.FaturaId,
                    cobranca.Mensagem,
                    status
                );
            }
        }


    }
}