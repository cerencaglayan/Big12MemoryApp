using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Big12MemoryApp.Application.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, int userId, CancellationToken ct = default);
    Task DeleteFileAsync(string filePath, CancellationToken ct = default);
    string GetFileUrl(string filePath);
    Task<Stream> DownloadFileAsync(string filePath, CancellationToken ct = default);
}


