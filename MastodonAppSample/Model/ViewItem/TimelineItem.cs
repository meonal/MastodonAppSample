using System;
using Mastonet.Entities;

namespace MastodonAppSample.Model.ViewItem
{
    public class TimelineItem
    {
        public TimelineItem()
        {
        }

        public Status Staus { get; set; }
        public byte[] IconImage { get; set; }
    }
}
