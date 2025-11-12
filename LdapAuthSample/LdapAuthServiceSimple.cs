using Microsoft.Extensions.Options;
using System.DirectoryServices;
using ZLogger;

namespace LdapAuthSample;

/// <summary>
/// シンプルな認証処理。DirectoryEntryを用います
/// </summary>
public class LdapAuthServiceSimple
{
    private readonly LdapOptions _options;
    private readonly Microsoft.Extensions.Logging.ILogger<LdapAuthServiceSimple> _logger;
    /// <summary>
    /// LdapAuthServiceSimple の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">LDAP接続オプション</param>
    /// <param name="logger">ILogger</param>
    public LdapAuthServiceSimple(IOptions<LdapOptions> options, Microsoft.Extensions.Logging.ILogger<LdapAuthServiceSimple> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// 指定したユーザーIDとパスワードでLDAP認証を行います。
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <param name="password">パスワード</param>
    /// <returns>認証成功時はtrue、失敗時はfalse</returns>
    public bool Authenticate(string userId, string password)
    {
        try
        {
            using var entry = new DirectoryEntry($"LDAP://{_options.Server}", userId, password);
            _ = entry.NativeObject;
            // 例外が発生しなければ認証成功
            return true;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex, $"LDAP認証失敗: {ex.Message}");
            return false;
        }
    }
}
