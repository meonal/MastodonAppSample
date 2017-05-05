using System;
using Mastonet.Entities;

namespace MastodonAppSample.Model.Entity
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
