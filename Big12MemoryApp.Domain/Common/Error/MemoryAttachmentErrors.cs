namespace Big12MemoryApp.Domain.Common.Error;

public class MemoryAttachmentErrors
{
    public static Big12MemoryApp.Domain.Common.Error.Error MemoryAttachmentNotFound(int memoryAttachmentId) => Big12MemoryApp.Domain.Common.Error.Error.BadRequest("400", $"Anıya bağlı belge '{memoryAttachmentId}' bulunamadı");
}