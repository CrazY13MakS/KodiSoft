using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
    public class FeedOutput : ISyndicationFeedOutput
    {
        public FeedOutput()
        {
            Categories = new List<ISyndicationCategory>();
            Items = new List<ISyndicationItem>();
            Links = new List<ISyndicationLink>();
            Persons = new List<ISyndicationPerson>();
            Content = new List<ISyndicationContent>();
            Images = new List<ISyndicationImage>();
            SourceType = SourceType.Unknown;
        }
        public ICollection<ISyndicationCategory> Categories { get; set; }
        public ICollection<ISyndicationItem> Items { get; set; }
        public ICollection<ISyndicationLink> Links { get; set; }
        public ICollection<ISyndicationPerson> Persons { get; set; }
        public ICollection<ISyndicationContent> Content { get; set; }
        public ICollection<ISyndicationImage> Images { get; set; }
        public SourceType SourceType { get; set;}
    }
}
