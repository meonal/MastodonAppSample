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
        int mPage;
        readonly HttpClient client;
        List<TimelineItem> timeline;
        TimelineAdapter adapter;
        TimelineStreaming streaming;

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

            var mastodonClient = new ApiClient().Create();
            switch ((MainTab)mPage)
            {
                case MainTab.Main:
                    streaming = mastodonClient.GetUserStreaming();
                    break;
                case MainTab.Local:
                    streaming = mastodonClient.GetHashtagStreaming("pixiv");
                    break;
                case MainTab.Federation:
                    streaming = mastodonClient.GetPublicStreaming();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            streaming.OnUpdate += OnStreamingUpdate;
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.fragment_page, container, false);
            var listView = (ListView)view;
            listView.Adapter = adapter;

            streaming.Start();
            Debug.WriteLine("Start:" + mPage);

            //行の移動
            //listView.SetSelection(10);  //10番目の行(item_10)を一番上に表示する

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            streaming.Stop();
            Debug.WriteLine("Stop:" + mPage);
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
