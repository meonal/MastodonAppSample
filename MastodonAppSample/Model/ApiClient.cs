using System.Net;
using System.Threading.Tasks;
using MastodonAppSample.Model.Definition;
using MastodonAppSample.Model.Repository;
using Mastonet;
using Mastonet.Entities;
using Newtonsoft.Json;

namespace MastodonAppSample.Model
{
    /// <summary>
    /// APIクライアントの登録・作成
    /// </summary>
    public class ApiClient
    {
        const string redirectUrl = Constants.AppUrlScheme + "://" + Constants.AppUrlHostAuthorize;

        private readonly SettingRepository setting;
        private readonly AuthenticationClient authClient;
        private readonly string instance;
        private readonly string appRegistrationKey;
        private readonly string authKey;

        public bool HasAccessToken
        {
            get { return !string.IsNullOrEmpty(setting.GetString(authKey)); }
        }

        public ApiClient(string instance = null)
        {
            setting = new SettingRepository(Constants.AppName);

            this.instance = instance ?? (setting.GetString(AppSettingKeys.CurrentInstance) ?? string.Empty);

            // インスタンス毎に設定保存（将来のインスタンス切替に備えて）
            appRegistrationKey = $"{this.instance}.{AppSettingKeys.AppRegistration}";
            authKey = $"{this.instance}.{AppSettingKeys.Auth}";

            authClient = new AuthenticationClient(this.instance);
        }

        public async Task<string> Register()
        {
            var appRegistration = await authClient.CreateApp(Constants.AppName, Scope.Read | Scope.Write | Scope.Follow, redirectUri: redirectUrl);
            setting.Set(appRegistrationKey, JsonConvert.SerializeObject(appRegistration));

            // ブラウザでの認可後にアプリに戻ってこれるようにURLスキーマをリダイレクトURLにセット
            return authClient.OAuthUrl(redirectUrl);
        }

        public async Task Auth(string authCode)
        {
            var auth = await authClient.ConnectWithCode(authCode, redirectUrl);
            setting.Set(authKey, JsonConvert.SerializeObject(auth));
            setting.Set(AppSettingKeys.CurrentInstance, instance);
        }

        public MastodonClient Create()
        {
            var appRegistration = JsonConvert.DeserializeObject<AppRegistration>(setting.GetString(appRegistrationKey));
            var auth = JsonConvert.DeserializeObject<Auth>(setting.GetString(authKey));
            appRegistration.Instance = instance;
            return new MastodonClient(appRegistration, auth);
        }


    }
}