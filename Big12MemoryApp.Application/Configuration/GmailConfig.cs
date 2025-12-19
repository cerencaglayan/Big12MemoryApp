namespace Big12MemoryApp.Application.Configuration;

public class GmailConfig
{
    public const string GmailOptionKey = "GmailOptions";

    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string Email { get; set; } = "noreply.big12@gmail.com";
    public string Password { get; set; } = string.Empty;
}


