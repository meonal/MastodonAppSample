using System;
using Android.App;
using Android.Content;
using MastodonAppSample.Model.Repository;
using Mastonet;
using NUnit.Framework;

namespace MastodonAppSample.Test
{
    [TestFixture]
    public class MastodonApiTest
    {
        const string AppName = "Paooooooon";
        const string InstanceUrl = "friends.nico";

        private SettingRepository setting;

        [SetUp]
        public void Setup()
        {
            setting = new SettingRepository(AppName + ".Test");
        }


        [TearDown]
        public void Tear() { }

        [Test]
        public async void OAuthApp登録()
        {
            var prefs = Application.Context.GetSharedPreferences(AppName, FileCreationMode.Private);
            if (!string.IsNullOrEmpty(prefs.GetString("ClientId", null)))
            {
                Assert.Ignore("すでに設定が保存されているためスキップ");
            }

            var authClient = new AuthenticationClient(InstanceUrl);
            var appRegistration = await authClient.CreateApp("Paoooooon", Scope.Read | Scope.Write | Scope.Follow);

            var editor = prefs.Edit();
            editor.PutString("ClientId", appRegistration.ClientId);
            editor.PutString("ClientSecret", appRegistration.ClientSecret);
            editor.Commit();

            Console.WriteLine("appRegistration");
            Console.WriteLine("Id:" + appRegistration.Id);
            Console.WriteLine("RedirectUri:" + appRegistration.RedirectUri);
            Console.WriteLine("ClientId:" + appRegistration.ClientId);
            Console.WriteLine("ClientSecret:" + appRegistration.ClientSecret);

            Assert.True(true);
        }

        [Test]
        public void OAuthApp設定取得()
        {

            var prefs = Application.Context.GetSharedPreferences(AppName, FileCreationMode.Private);
            var clientId = prefs.GetString("ClientId", null);
            var clientSecret = prefs.GetString("ClientSecret", null);

            Console.WriteLine("appRegistration");
            Console.WriteLine("ClientId:" + clientId);
            Console.WriteLine("ClientSecret:" + clientSecret);

            Assert.AreNotEqual(null, clientId);
            Assert.AreNotEqual(null, clientSecret);
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
