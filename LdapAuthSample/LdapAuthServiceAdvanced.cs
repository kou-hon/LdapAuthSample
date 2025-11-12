using ZLogger;
using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;

namespace LdapAuthSample;

/// <summary>
/// �g���I�v�V�����𗘗p����LDAP�F�؃T�[�r�X�B
/// </summary>
public class LdapAuthServiceAdvanced
{
    private readonly LdapOptions _options;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    /// <summary>
    /// LdapAuthServiceAdvanced �̐V�����C���X�^���X�����������܂��B
    /// </summary>
    /// <param name="options">�g��LDAP�ڑ��I�v�V����</param>
    /// <param name="logger">ILogger</param>
    public LdapAuthServiceAdvanced(IOptions<LdapOptions> options, Microsoft.Extensions.Logging.ILogger<LdapAuthServiceAdvanced> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// �w�肵�����[�U�[ID�ƃp�X���[�h��LDAP�F�؂��s���܂��B
    /// </summary>
    /// <param name="userId">���[�U�[ID</param>
    /// <param name="password">�p�X���[�h</param>
    /// <returns>�F�ؐ�������true�A���s����false</returns>
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
            _logger.ZLogError(ex, $"LDAP詳細認証失敗: {ex.Message}");
            return false;
        }
    }
}
