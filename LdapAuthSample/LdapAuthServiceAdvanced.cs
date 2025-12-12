using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;
using ZLogger;

namespace LdapAuthSample;

/// <summary>
/// グローバルオプションを利用したLDAP認証サービス。
/// </summary>
public class LdapAuthServiceAdvanced
{
    private readonly LdapOptions _options;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    /// <summary>
    /// LdapAuthServiceAdvanced の新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="options">グローバルLDAP接続オプション</param>
    /// <param name="logger">ILogger</param>
    public LdapAuthServiceAdvanced(IOptions<LdapOptions> options, Microsoft.Extensions.Logging.ILogger<LdapAuthServiceAdvanced> logger)
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
            string userDn = null;
            using (var searchConnection = new LdapConnection(_options.Server))
            {
                searchConnection.SessionOptions.ProtocolVersion = 3;
                searchConnection.SessionOptions.StartTransportLayerSecurity(null);
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
            connection.SessionOptions.StartTransportLayerSecurity(null);
            connection.AuthType = AuthType.Basic;
            var credential = new NetworkCredential(userDn, password);
            connection.Bind(credential);
            return true;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex, $"LDAP詳細認証失敗: {ex.Message}");
            return false;
        }
    }
}
