# LdapAuthSample

LDAP/Active Directoryサーバーに対してユーザー認証を行うC#サンプル

## 機能
- `appsettings.json`でLDAPサーバ情報を管理
- ユーザーID・パスワードをコンソールから入力
- LDAP認証の成否を表示

## Active Directoryでの使い方

### appsettings.jsonの設定例
```json
{
  "LdapSettings": {
    "Server": "ad.example.com",          // ADサーバーのホスト名またはIP
    "BaseDn": "dc=example,dc=com",       // ADドメインのDN
    "BindUser": "hogehoge@example.com",  // ADサーバーの有効ユーザ
    "BindPassword": "password",
    "LoginIdAttribute": "sAMAccountName" // LoginIDとして利用する属性
 }
}
```

#### Active Directory用のBaseDn例
- ドメインが `example.co.jp` の場合: `dc=example,dc=co,dc=jp`

### 認証方法
1. `Input Account ID:` にActive Directoryのユーザー名（例: `123456` など）を入力
2. `Password:` にパスワードを入力（非表示で入力されます）
3. 認証結果が `Success` または `failed` と表示されます

## 参考
- [System.DirectoryServices.Protocols (Microsoft Docs)](https://learn.microsoft.com/dotnet/api/system.directoryservices.protocols)

