using ZLogger;
using Microsoft.Extensions.Options;
using System.DirectoryServices;

namespace LdapAuthSample;

/// <summary>
/// �V���v���ȔF�ؕ����ADirectoryEntry�ɂ��܂���
/// </summary>
public class LdapAuthServiceSimple
{
    private readonly LdapOptions _options;
    private readonly Microsoft.Extensions.Logging.ILogger<LdapAuthServiceSimple> _logger;
    /// <summary>
    /// LdapAuthServiceSimple �̐V�����C���X�^���X�����������܂��B
    /// </summary>
    /// <param name="options">LDAP�ڑ��I�v�V����</param>
    /// <param name="logger">ILogger</param>
    public LdapAuthServiceSimple(IOptions<LdapOptions> options, Microsoft.Extensions.Logging.ILogger<LdapAuthServiceSimple> logger)
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
            using var entry = new DirectoryEntry($"LDAP://{_options.Server}", userId, password);
            _  = entry.NativeObject;
            //��O�������Ȃ���ΔF�ؐ���
            return true;
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex, $"LDAP認証失敗: {ex.Message}");
            return false;
        }
    }
}
