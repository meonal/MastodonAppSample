using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using MastodonAppSample.Model.Entity;

using Context = Android.Content.Context;
using Debug = System.Diagnostics.Debug;

namespace MastodonAppSample.View
{
    /// <summary>
    /// タイムラインのListViewのAdapter
    /// </summary>
    public class TimelineAdapter : BaseAdapter<TimelineItem>
    {
        List<TimelineItem> items;
        readonly LayoutInflater inflater;


        public TimelineAdapter(Context context, List<TimelineItem> list)
        {
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            items = list;
        }

        public override TimelineItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {

            var item = items[position];

            //Debug.WriteLine("position: " + position);
            //Debug.WriteLine("name: " + item.Staus.Account.AccountName);

            var view = inflater.Inflate(Resource.Layout.timeline_item, null);

            var icon = view.FindViewById<ImageView>(Resource.Id.Icon);
            if (item.IconImage != null)
            {
                var bmp = Android.Graphics.BitmapFactory.DecodeByteArray(item.IconImage, 0, item.IconImage.Length);
                icon.SetImageBitmap(bmp);
            }

            var accountName = view.FindViewById<TextView>(Resource.Id.AccountName);
            accountName.Text = item.Staus.Account.AccountName;

            var content = view.FindViewById<TextView>(Resource.Id.Content);
            content.Text = item.Staus.Content;

            return view;
        }
    }
}
