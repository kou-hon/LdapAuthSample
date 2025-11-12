using ZLogger;
using LdapAuthSample;

namespace LdapAuthSample;


public class App
{
    private readonly LdapAuthServiceSimple _ldapAuthService;
    private readonly LdapAuthServiceAdvanced _ldapAuthServiceAdvanced;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    public App(LdapAuthServiceSimple ldapAuthService, LdapAuthServiceAdvanced ldapAuthServiceAdvanced, Microsoft.Extensions.Logging.ILogger<App> logger)
    {
        _ldapAuthService = ldapAuthService;
        _ldapAuthServiceAdvanced = ldapAuthServiceAdvanced;
        _logger = logger;
    }

    public Task RunAsync()
    {
        Console.Write("Input Account ID: ");
        var userId = Console.ReadLine();
        Console.Write("Password: ");
        var password = ReadPassword();

    _logger.ZLogInformation($"入力ID: {userId}");

    var result = _ldapAuthService.Authenticate(userId ?? string.Empty, password);
    _logger.ZLogInformation($"シンプル認証: {(result ? "Success" : "failed")}");
    Console.WriteLine(result ? "Success" : "failed");

    result = _ldapAuthServiceAdvanced.Authenticate(userId ?? string.Empty, password);
    _logger.ZLogInformation($"詳細認証: {(result ? "Success" : "failed")}");
    Console.WriteLine(result ? "Success" : "failed");
    return Task.CompletedTask;
    }

    private static string ReadPassword()
    {
        var pwd = string.Empty;
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                pwd += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
            {
                pwd = pwd[..^1];
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return pwd;
    }
}
