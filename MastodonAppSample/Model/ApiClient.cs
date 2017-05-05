using System.Threading.Tasks;
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
        const string AppName = "Paoooon";
        const string redirectUrl = "paoooon://authorize";

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
            setting = new SettingRepository(AppName);

            this.instance = instance ?? (setting.GetString("currentInstance") ?? string.Empty);
            appRegistrationKey = $"{this.instance}.appRegistration";
            authKey = $"{this.instance}.auth";

            authClient = new AuthenticationClient(this.instance);
        }

        public async Task<string> Register()
        {
            var appRegistration = await authClient.CreateApp(AppName, Scope.Read | Scope.Write | Scope.Follow, redirectUri:redirectUrl);
            setting.Set(appRegistrationKey, JsonConvert.SerializeObject(appRegistration));

            // ブラウザでの認可後にアプリに戻ってこれるようにURLスキーマをリダイレクトURLにセット
            return authClient.OAuthUrl(redirectUrl);
        }

        public async Task Auth(string authCode)
        {
            var auth = await authClient.ConnectWithCode(authCode, redirectUrl);
            setting.Set(authKey, JsonConvert.SerializeObject(auth));
            setting.Set("currentInstance", instance);
        }

        public MastodonClient Create()
        {
            var appRegistration = JsonConvert.DeserializeObject<AppRegistration>(setting.GetString(appRegistrationKey));
            var auth = JsonConvert.DeserializeObject<Auth>(setting.GetString(authKey));
            return new MastodonClient(appRegistration, auth);
        }


    }
}