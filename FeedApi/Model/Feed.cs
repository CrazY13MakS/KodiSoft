﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Model
{
    public class Feed
    {
        public long Id { get; set; }
        public String Uri { get; set; }
        public String Type { get; set; }
        public DateTime CreateddAt { get; set; }


        public virtual List<FeedCollection> FeedCollections { get; set; }
    }
}
