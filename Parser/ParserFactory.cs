using Parser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class ParserFactory : IFeedParserFactory
    {
        public ParserBase CreateParser(SourceType sourceType)
        {
            switch (sourceType)
            {

                case SourceType.XML:
                    break;
                case SourceType.Rss:
                    return new  RssFeedParser();
                case SourceType.Atom:
                    return new AtomFeedReader();
                case SourceType.HTML:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
