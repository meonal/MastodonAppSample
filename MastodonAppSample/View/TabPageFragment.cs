using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MastodonAppSample.Model;
using MastodonAppSample.Model.Entity;
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
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_page, container, false);
            var listView = (ListView)view;

            var timeline = new List<TimelineItem>();
            var adapter = new TimelineAdapter(Context, timeline);
            listView.Adapter = adapter;

            //行の移動
            //listView.SetSelection(10);  //10番目の行(item_10)を一番上に表示する

            if (mPage == 1)
            {
                var mastodonClient = new ApiClient().Create();

                var streaming = mastodonClient.GetPublicStreaming();
                streaming.OnUpdate += async (sender, e) =>
                {
                    Debug.WriteLine(e.Status.Account.AvatarUrl);
                    var item = new TimelineItem();

                    try
                    {
                        item.IconImage = await client.GetByteArrayAsync(e.Status.Account.AvatarUrl);
                    }
                    catch (System.Exception)
                    {
                        Debug.WriteLine("アイコン画像取得失敗");
                    }

                    item.Staus = e.Status;
                    timeline.Insert(0, item);
                    adapter.NotifyDataSetChanged();
                };

                streaming.Start();
            }

            return view;
        }


    }
}
