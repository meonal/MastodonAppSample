using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MastodonAppSample.View
{
    /// <summary>
    /// タブページャー
    /// </summary>
    public class TabPagerAdapter : FragmentPagerAdapter
    {
        const int PAGE_COUNT = 3;
        private string[] tabTitles = 
        {
            "メイン",
            "ローカル",
            "連合"
        };
        readonly Context context;

        public TabPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TabPagerAdapter(Context context, FragmentManager fm) : base(fm)
        {
            this.context = context;
        }

        public override int Count 
        {
            get { return PAGE_COUNT; }
        }

        public override Fragment GetItem(int position)
        {
            return TabPageFragment.newInstance(position + 1);
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            // Generate title based on item position
            return CharSequence.ArrayFromStringArray(tabTitles)[position];
        }

        public Android.Views.View GetTabView(int position)
        {
            // Given you have a custom layout in `res/layout/custom_tab.xml` with a TextView
            var tv = (TextView) LayoutInflater.From(context).Inflate(Resource.Layout.custom_tab, null);
            tv.Text = tabTitles[position];
            return tv;
        }
    }
}
