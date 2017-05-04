using System;
using Android.App;
using Android.Content;

namespace MastodonAppSample.Model.Repository
{
    /// <summary>
    /// 設定の保存・取得
    /// </summary>
    public class SettingRepository
    {
        private readonly ISharedPreferences prefs;

        public SettingRepository() : this("DefaultAppName") { }

        public SettingRepository(string appName)
        {
            prefs = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);
        }

        public string GetString(string key) => prefs.GetString(key, default(string));
        public int GetInt(string key) => prefs.GetInt(key, default(int));
        public long GetLong(string key) => prefs.GetLong(key, default(long));
        public float GetFloat(string key) => prefs.GetFloat(key, default(float));
        public bool GetBoolean(string key) => prefs.GetBoolean(key, default(bool));

        public void Set(string key, string value)
        {
            var editor = prefs.Edit();
            editor.PutString(key, value);
            editor.Commit();
        }
        public void Set(string key, int value)
        {
            var editor = prefs.Edit();
            editor.PutInt(key, value);
            editor.Commit();
        }
        public void Set(string key, long value)
        {
            var editor = prefs.Edit();
            editor.PutLong(key, value);
            editor.Commit();
        }
        public void Set(string key, float value)
        {
            var editor = prefs.Edit();
            editor.PutFloat(key, value);
            editor.Commit();
        }
        public void Set(string key, bool value)
        {
            var editor = prefs.Edit();
            editor.PutBoolean(key, value);
            editor.Commit();
        }

    }
}