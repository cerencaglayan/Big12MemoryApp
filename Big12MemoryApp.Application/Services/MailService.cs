using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Big12MemoryApp.Application.Configuration;
using Big12MemoryApp.Application.DTO.Requests;
using Microsoft.Extensions.Options;

namespace Big12MemoryApp.Application.Services;

public class MailService : IMailService
{
    private readonly GmailConfig _config;
    
    public MailService(IOptions<GmailConfig> gmailConfig)
    {
        _config = gmailConfig.Value;
    }

    public async Task SendMailAsync(SendMailRequest request)
    {
        MailMessage mailMessage = new MailMessage()
        {
            From = new MailAddress(_config.Email),
            Subject = request.Subject,
            Body = request.Body,
        };
        
        mailMessage.To.Add(request.Recipient);
        
        using var smptClient = new SmtpClient();
        smptClient.Host ="smtp.gmail.com";
        smptClient.Port = 587;
        smptClient.Credentials = new System.Net.NetworkCredential(_config.Email, _config.Password);
        smptClient.EnableSsl = true;

        try
        {
            await smptClient.SendMailAsync(mailMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        } 
        

    }
}