using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Model
{
    public interface IFeedWriter
    {
        Task<String> Write(ISyndicationFeedOutput syndicationFeed); 
    }
}
