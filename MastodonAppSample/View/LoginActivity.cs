using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MastodonAppSample.Model;
using MastodonAppSample.Definition;
using MastodonAppSample.View;

using Debug = System.Diagnostics.Debug;

namespace MastodonAppSample
{
    /// <summary>
    /// Login activity.
    /// OAuthの認証で、アプリ ｰ> ブラウザ ｰ> アプリと遷移する。
    /// ブラウザからアプリを呼ぶためIntentFilterの属性を追加している。
    /// </summary>
    [Activity(Label = Constants.AppName,
              MainLauncher = true,
              LaunchMode = Android.Content.PM.LaunchMode.SingleTask,
              Icon = "@drawable/icon")]
    [IntentFilter(new[] { Intent.ActionView },
                  Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
                  DataScheme = Constants.AppUrlScheme,
                  DataHost = Constants.AppUrlHostAuthorize)]
    public class LoginActivity : AppCompatActivity
    {
        private ApiClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.login);

            // すでにアクセストークンを持っている場合はログイン画面はスキップ
            if (new ApiClient().HasAccessToken)
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            }

            var instanceEdit = FindViewById<EditText>(Resource.Id.instanceEdit);
            var loginButton = FindViewById<Button>(Resource.Id.loginButton);

            var instance = instanceEdit.Text;

            loginButton.Click += async (sender, e) =>
            {
                Debug.WriteLine("APIクライアントの登録開始 instance: " + instance);

                client = new ApiClient(instance);
                var url = await client.Register();

                var uri = Android.Net.Uri.Parse(url);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

        }

        /// <summary>
        /// ブラウザから戻ってきた後の処理
        /// </summary>
        /// <param name="intent">Intent.</param>
        protected override async void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var uri = intent.Data;

            Debug.WriteLine("uri: " + uri);

            var m = Regex.Match(uri.ToString(), "code=(.*)");
            if (!m.Success) return;
            var authCode = m.Groups[1].Value;

            Debug.WriteLine("authCode: " + authCode);

            if (client == null) return;
            await client.Auth(authCode);

            var main = new Intent(this, typeof(MainActivity));
            StartActivity(main);
        }
    }
}
