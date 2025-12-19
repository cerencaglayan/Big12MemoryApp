using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Application.DTO.Requests;
using Big12MemoryApp.Application.Services;
using Big12MemoryApp.Domain.Entities;
using Big12MemoryApp.Domain.Repositories;

namespace Big12MemoryApp.Infrastructure.Configuration.Jobs;

public class MailJob(IUserRepository userRepository, IMailService mailService)
{
   
    
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var birthdayUsers = await CheckBirthdays(cancellationToken);
        
        if (!birthdayUsers.Any())
            return;
        
        var allUsers = await userRepository.GetAllAsync(cancellationToken);
        
        var usersToNotify = allUsers
            .Where(u => birthdayUsers.All(b => b!.Id != u.Id))
            .ToList();
        
        foreach (var birthdayUser in birthdayUsers)
        {
            var mailBody = PrepareMailBody(birthdayUser!);

            foreach (var receiver in usersToNotify)
            {
                if (receiver.Email is null)
                {
                    continue;
                }
                
                SendMailRequest request = new SendMailRequest();
                request.Recipient = receiver.Email;
                request.Body = mailBody;
                request.Subject = "🎉 Doğum Günü Hatırlatması";
                
                await mailService.SendMailAsync(request);
            }
        }
        
    }

    private async Task<List<User?>> CheckBirthdays(CancellationToken cancellationToken)
    {
        return await userRepository.GetByBirthdayAsync(DateTime.Today, cancellationToken);

    }

    private string PrepareMailBody(User user)
    {
        if (user.Birthday != null)
            return $"""
                    🎂 Bugün uygulamadaki kullanıcılardan birinin doğum günü!

                    🧑 İyi ki doğdun {user.Name} !

                    {user.Birthday.Value.Year - DateTime.Today.Year}.yaşın sana uğur getirsin!

                    Kendisine güzel bir kutlama iletmeyi unutma! 🎉
                    """;
        return "Hata";
    }

    
    
    
    
}