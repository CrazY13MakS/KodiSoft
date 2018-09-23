using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Model
{
   public abstract class ParserBase:IParser
    {
        public  IEnumerable<SourceType> SupportedTypes { get; protected set; }


        public ParserBase(IEnumerable<SourceType> supportedTypes )
        {
            SupportedTypes = supportedTypes ?? throw new ArgumentNullException(nameof(supportedTypes));
        }

        public abstract  Task<ISyndicationFeedOutput> ParseAsync(String path);

        public abstract  bool TryParse(String path, out ISyndicationFeedOutput result);

    }
}
