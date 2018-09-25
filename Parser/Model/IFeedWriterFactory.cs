using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Model
{
   public interface IFeedWriterFactory
    {
        IFeedWriter FeedWriter(OutputType outputType);
    }
}
