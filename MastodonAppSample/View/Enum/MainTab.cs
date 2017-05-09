using System;
namespace MastodonAppSample.View.Enum
{
    public enum MainTab
    {
        Main = 1,
        Local,
        Federation
    }

    public static class MainTabExtension
    {
        public static string ToText(this MainTab tab)
        {
            switch (tab)
            {
                case MainTab.Main:
                    return "メイン";
                case MainTab.Local:
                    return "ローカル";
                case MainTab.Federation:
                    return "連合";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
