using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
   public interface ISyndicationFeedOutput
    {
        ICollection<ISyndicationCategory> Categories { get; set; }
        ICollection<ISyndicationItem> Items { get; set; }
        ICollection<ISyndicationLink> Links { get; set; }
        ICollection<ISyndicationPerson> Persons { get; set; }
        ICollection<ISyndicationContent> Content { get; set; }
        ICollection<ISyndicationImage> Images { get; set; }
        SourceType SourceType { get; set; }

    }
}
