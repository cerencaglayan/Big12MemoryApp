namespace Big12MemoryApp.Domain.Common.Error;

public class MemoryErrors
{
    public static Big12MemoryApp.Domain.Common.Error.Error MemoryNotfound(int id) => Big12MemoryApp.Domain.Common.Error.Error.BadRequest("400", $"Anı '{id}' bulunamadı");

    
}