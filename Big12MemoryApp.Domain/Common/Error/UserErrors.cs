namespace Big12MemoryApp.Domain.Common.Error;

public class UserErrors
{
    public static Big12MemoryApp.Domain.Common.Error.Error InvalidCurrentPassword() => Big12MemoryApp.Domain.Common.Error.Error.BadRequest("400", "Hatalı Şifre girdiniz");
    public static Big12MemoryApp.Domain.Common.Error.Error UserNotfound(string name) => Big12MemoryApp.Domain.Common.Error.Error.BadRequest("400", $"Kullanıcı '{name}' bulunamadı");
    public static Big12MemoryApp.Domain.Common.Error.Error Unauthorized(int userId) => Big12MemoryApp.Domain.Common.Error.Error.BadRequest("401", $"Kullanıcı no '{userId}'' : bu işlem için yetkisi yok");
    
}