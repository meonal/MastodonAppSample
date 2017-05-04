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
        const string AppName = "Paooooooon";

        private readonly SettingRepository setting;
        private readonly string instance;
        private string appRegistrationKey;
        private string authKey;

        public ApiClient()
        {
            setting = new SettingRepository(AppName);
            instance = setting.GetString("currentInstance");
            SetSettingKey();
        }

        public ApiClient(string instance)
        {
            setting = new SettingRepository(AppName);            
            this.instance = instance;
            SetSettingKey();
        }

        private void SetSettingKey()
        {
            appRegistrationKey = $"{instance}.appRegistration";
            authKey = $"{instance}.auth"; 
        }

        public async Task<string> Register()
        {
            var authClient = new AuthenticationClient(instance);
            var appRegistration = await authClient.CreateApp(AppName, Scope.Read | Scope.Write | Scope.Follow);
            setting.Set(appRegistrationKey, JsonConvert.SerializeObject(appRegistration));
            return authClient.OAuthUrl();
        }

        public async Task Auth(string authCode)
        {
            var authClient = new AuthenticationClient(instance);
            var auth = await authClient.ConnectWithCode(authCode);
            setting.Set(authKey, JsonConvert.SerializeObject(auth));
        }

        public MastodonClient Create()
        {
            var appRegistration = JsonConvert.DeserializeObject<AppRegistration>(setting.GetString(appRegistrationKey));
            var auth = JsonConvert.DeserializeObject<Auth>(setting.GetString(authKey));
            return new MastodonClient(appRegistration, auth);
        }
    }
}