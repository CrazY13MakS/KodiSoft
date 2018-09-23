using System;
using System.Threading.Tasks;

namespace Parser.Model
{
    public interface IParser
    {
        Task<ISyndicationFeedOutput> ParseAsync(String path);
         bool TryParse(String path, out ISyndicationFeedOutput result);
    }
}
