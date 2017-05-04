
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MastodonAppSample.Model;

namespace MastodonAppSample
{
    
    [Activity(Label = "Paoooon", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.login);

            var instanceEdit = FindViewById<EditText>(Resource.Id.instanceEdit);
            var loginButton = FindViewById<Button>(Resource.Id.loginButton);

            loginButton.Click += async (sender, e) =>
            {
                var client = new ApiClient(instanceEdit.Text);
                var url = await client.Register();

                var uri = Android.Net.Uri.Parse(url);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

        }
    }
}
