using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MastodonAppSample.Model;
using MastodonAppSample.Model.ViewItem;
using MastodonAppSample.View.Enum;
using Mastonet;
using Debug = System.Diagnostics.Debug;

namespace MastodonAppSample.View
{
    /// <summary>
    /// タブページ
    /// </summary>
    public class TabPageFragment : Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        private readonly HttpClient client;
        private List<TimelineItem> timeline;
        private TimelineAdapter adapter;

        public TabPageFragment()
        {
            client = new HttpClient();
        }

        public static TabPageFragment newInstance(int page)
        {
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new TabPageFragment();
            fragment.Arguments = args;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);

            timeline = new List<TimelineItem>();
            adapter = new TimelineAdapter(Context, timeline);
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_page, container, false);
            var listView = (ListView)view;

            listView.Adapter = adapter;

            //行の移動
            //listView.SetSelection(10);  //10番目の行(item_10)を一番上に表示する

            var mastodonClient = new ApiClient().Create();
            switch ((MainTab)mPage)
            {
                case MainTab.Main:
                    var uStreaming = mastodonClient.GetUserStreaming();
                    uStreaming.OnUpdate += OnStreamingUpdate;
                    uStreaming.Start();
                    break;
                case MainTab.Local:
                    var lStreaming = mastodonClient.GetHashtagStreaming("Xamarin");
                    lStreaming.OnUpdate += OnStreamingUpdate;
                    lStreaming.Start();
                    break;
                case MainTab.Federation:
                    var fStreaming = mastodonClient.GetPublicStreaming();
                    fStreaming.OnUpdate += OnStreamingUpdate;
                    fStreaming.Start();
                    break;

            }

            return view;
        }

        private async void OnStreamingUpdate(object sender, StreamUpdateEventArgs e)
        {
            //Debug.WriteLine(e.Status.Account.AvatarUrl);
            var item = new TimelineItem();

            try
            {
                item.IconImage = await client.GetByteArrayAsync(e.Status.Account.AvatarUrl);
            }
            catch (Exception)
            {
                Debug.WriteLine("アイコン画像取得失敗");
            }

            item.Staus = e.Status;
            timeline.Insert(0, item);
            adapter.NotifyDataSetChanged();
        }


    }
}
