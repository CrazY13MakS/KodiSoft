using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
    public interface IFeedParserFactory
    {
        ParserBase CreateParser(SourceType sourceType);
    }
}
