using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mastonet;
using NUnit.Framework;

namespace MastodonAppSample.Test
{
    [TestFixture]
    public class MastodonApiTest
    {

        [SetUp]
        public void Setup() { }


        [TearDown]
        public void Tear() { }

        [Test]
        public async void APIクライアント登録()
        {
            //var client = new HttpClient();
            //var text = await client.GetStringAsync("https://www.yahoo.co.jp/");
            //Console.WriteLine(text);

            var authClient = new AuthenticationClient("pawoo.net");
            var appRegistration = await authClient.CreateApp("Paoooooon", Scope.Read | Scope.Write | Scope.Follow);

            Console.WriteLine("appRegistration");

            Console.WriteLine("Id:" + appRegistration.Id);
            Console.WriteLine("RedirectUri:" + appRegistration.RedirectUri);
            Console.WriteLine("ClientId:" + appRegistration.ClientId);
            Console.WriteLine("ClientSecret:" + appRegistration.ClientSecret);

            Assert.True(true);
        }

    }
}
