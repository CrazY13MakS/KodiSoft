using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Model
{
    public class FeedCollectionsFeed
    {
        public long Id { get; set; }
        public long FeedId { get; set; }
        public long FeedCollectionId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Feed Feed { get; set; }
        public virtual FeedCollection FeedCollection { get; set; }
    }
}
