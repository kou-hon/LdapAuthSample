using Microsoft.Extensions.Options;

namespace LdapAuthSample;

/// <summary>
/// LDAP接続設定の基本オプションを表します。
/// </summary>
public class LdapOptions
{
    /// <summary>
    /// LDAPサーバーのアドレス。
    /// </summary>
    public string Server { get; set; } = string.Empty;
    /// <summary>
    /// 検索のベースDN。
    /// </summary>
    public string BaseDn { get; set; } = string.Empty;

    /// <summary>
    /// バインドユーザー名。
    /// </summary>
    public string BindUser { get; set; } = string.Empty;
    /// <summary>
    /// バインドユーザーのパスワード。
    /// </summary>
    public string BindPassword { get; set; } = string.Empty;

    /// <summary>
    /// ログインID属性名。
    /// </summary>
    public string LoginIdAttribute { get; set; } = "sAMAccountName";
}
