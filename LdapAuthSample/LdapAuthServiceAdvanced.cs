using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LdapAuthSample;

/// <summary>
/// 拡張オプションを利用したLDAP認証サービス。
/// </summary>
public class LdapAuthServiceAdvanced
{
    private readonly LdapOptions _options;
    /// <summary>
    /// LdapAuthServiceAdvanced の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">拡張LDAP接続オプション</param>
    public LdapAuthServiceAdvanced(IOptions<LdapOptions> options) => _options = options.Value;

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
            string userDn = null;
            using (var searchConnection = new LdapConnection(_options.Server))
            {
                searchConnection.SessionOptions.ProtocolVersion = 3;
                searchConnection.AuthType = AuthType.Basic;
                searchConnection.Bind(new NetworkCredential(_options.BindUser, _options.BindPassword));

                var searchRequest = new SearchRequest(
                    _options.BaseDn,
                    $"({_options.LoginIdAttribute}={userId})",
                    System.DirectoryServices.Protocols.SearchScope.Subtree,
                    null);
                var searchResponse = (SearchResponse)searchConnection.SendRequest(searchRequest);
                if (searchResponse.Entries.Count == 0)
                    return false;
                userDn = searchResponse.Entries[0].DistinguishedName;
            }
            using var connection = new LdapConnection(_options.Server);
            connection.SessionOptions.ProtocolVersion = 3;
            connection.AuthType = AuthType.Basic;
            var credential = new NetworkCredential(userDn, password);
            connection.Bind(credential);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
