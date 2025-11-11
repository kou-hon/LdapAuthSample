using Microsoft.Extensions.Options;
using System.DirectoryServices;

namespace LdapAuthSample;

/// <summary>
/// シンプルな認証方式、DirectoryEntryにおまかせ
/// </summary>
public class LdapAuthServiceSimple
{
    private readonly LdapOptions _options;
    /// <summary>
    /// LdapAuthServiceSimple の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">LDAP接続オプション</param>
    public LdapAuthServiceSimple(IOptions<LdapOptions> options) => _options = options.Value;

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
            _  = entry.NativeObject;
            //例外発生しなければ認証成功
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
