using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Model
{
    public class FeedCollection
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }
        public String UserId { get; set; }

        public virtual ICollection<FeedCollectionsFeed> FeedCollections { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
