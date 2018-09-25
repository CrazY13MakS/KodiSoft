using Parser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    class FeedWriterFactory : IFeedWriterFactory
    {
        public IFeedWriter FeedWriter(OutputType outputType)
        {
            switch (outputType)
            {
                case OutputType.Rss:
                    return new FeedToRssStringWriter();
                case OutputType.Atom:
                    return new FeedToAtomStringWriter();
                case OutputType.XML:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
