using System.Net;
using System.Net.Mail;

namespace CobrAI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> EnviarEmail(
            string destinatario,
            string mensagem)
        {
            try
            {
                var remetente = _configuration["EmailSettings:Remetente"];
                var senha = _configuration["EmailSettings:Senha"];

                using var email = new MailMessage();

                email.From = new MailAddress(remetente!);
                email.To.Add(destinatario);
                email.Subject = "CobrAI - Aviso de pendência";
                email.Body = mensagem;

                using var smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials = new NetworkCredential(
                    remetente,
                    senha
                );

                smtp.EnableSsl = true;

                await smtp.SendMailAsync(email);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");

                return false;
            }
        }
    }
}