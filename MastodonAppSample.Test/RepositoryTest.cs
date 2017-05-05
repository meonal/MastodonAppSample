using System;
using Android.App;
using Android.Content;
using MastodonAppSample.Model.Repository;
using Mastonet;
using Mastonet.Entities;
using NUnit.Framework;

namespace MastodonAppSample.Test
{
    [TestFixture]
    public class MastodonApiTest
    {
        const string AppName = "Paooon";
        const string redirectUrl = "paoooon://authorize";


        private SettingRepository setting;

        [SetUp]
        public void Setup()
        {
            setting = new SettingRepository(AppName + ".Test");
        }


        [TearDown]
        public void Tear() { }


        /// <summary>
        /// OAuth app登録.
        /// インスタンスによっては登録を許可していない
        /// </summary>
        /// <param name="instanceUrl">Instance URL.</param>
        [TestCase("friends.nico")]
        [TestCase("mstdn.jp")]
        [TestCase("pawoo.net")]
        [TestCase("mastodon.cloud")]
        public void OAuthApp登録(string instanceUrl)
        {
            
            //Assert.Ignore("毎回実行すると迷惑かかるのでスキップ");

            var authClient = new AuthenticationClient(instanceUrl);
            var appRegistration = new AppRegistration();
            Assert.DoesNotThrow(async () =>
            {
                appRegistration = await authClient.CreateApp(AppName, Scope.Read | Scope.Write | Scope.Follow, redirectUri:redirectUrl);
            });

            Console.WriteLine("appRegistration");
            Console.WriteLine("Id:" + appRegistration.Id);
            Console.WriteLine("RedirectUri:" + appRegistration.RedirectUri);
            Console.WriteLine("ClientId:" + appRegistration.ClientId);
            Console.WriteLine("ClientSecret:" + appRegistration.ClientSecret);
        }


        [Test]
        public void 設定保存_設定取得()
        {
            setting.Set("string", "1hoge");
            setting.Set("int", 2);
            setting.Set("float", (float)3.3);
            setting.Set("bool", true);

            Assert.AreEqual("1hoge", setting.GetString("string"));
            Assert.AreEqual(2, setting.GetInt("int"));
            Assert.AreEqual((float)3.3, setting.GetFloat("float"));
            Assert.AreEqual(true, setting.GetBoolean("bool"));
        }

    }
}
