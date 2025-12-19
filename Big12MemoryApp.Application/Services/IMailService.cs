using System.Threading.Tasks;
using Big12MemoryApp.Application.DTO.Requests;

namespace Big12MemoryApp.Application.Services;

public interface IMailService
{
    Task SendMailAsync(SendMailRequest request);
}


