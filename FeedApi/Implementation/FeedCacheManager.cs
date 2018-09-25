using FeedApi.Model;
using FeedApi.Services;
using Microsoft.Extensions.Caching.Memory;
using Parser;
using Parser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Implementation
{
    public class FeedCacheManager : ICacheManager<ISyndicationFeedOutput, long>
    {
        IMemoryCache _memoryCache;
        IRepository<Feed> _feedRepository;
        IFeedParserFactory _parserFactory;
        public FeedCacheManager(IMemoryCache memoryCache, IRepository<Feed> repository, IFeedParserFactory parserFactory)
        {
            _memoryCache = memoryCache;
            _feedRepository = repository;
            _parserFactory = parserFactory;
        }

        public async Task<ISyndicationFeedOutput> GetItemAsync(long elemId)
        {
            var feedItem = await _feedRepository.GetById(elemId);
            if (feedItem == null)
            {
                return null;
            }
            //var parser=  _parserFactory.CreateParser(TypeConverter.GetSourceType(feedItem.Type));
            // var result = await parser.ParseAsync(feedItem.Uri);

            return await _memoryCache.GetOrCreateAsync<ISyndicationFeedOutput>(feedItem.Uri, (x) =>
            {
                x.SlidingExpiration = TimeSpan.FromMinutes(30);
                var parser = _parserFactory.CreateParser(TypeConverter.GetSourceType(feedItem.Type));
                return parser.ParseAsync(feedItem.Uri);
            });

        }
    }
}
