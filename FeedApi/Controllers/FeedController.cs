using FeedApi.Model;
using FeedApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IRepository<Feed> _feedRepository;
        private readonly IRepository<FeedCollection> _feedCollectionRepository;
        private readonly IRepository<FeedCollectionsFeed> _feedCollectionsFeedRepository;
        private readonly ICacheManager<ISyndicationFeedOutput, long> _cacheManagerInput;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly String _userId;

        public FeedController(
            IRepository<Feed> feedRepository,
            IRepository<FeedCollection> feedCollectionRepository,
            IRepository<FeedCollectionsFeed> feedCollectionsFeedRepository,
            ICacheManager<ISyndicationFeedOutput, long> cacheManagerInput,
             UserManager<ApplicationUser> userManager)
        {
            _cacheManagerInput = cacheManagerInput;
            _feedCollectionRepository = feedCollectionRepository;
            _feedRepository = feedRepository;
            _feedCollectionsFeedRepository = feedCollectionsFeedRepository;
            _userManager = userManager;
            _userId = _userManager.GetUserId(User);
        }


        [HttpPost("api/feed/createcollection")]
        public async Task<IActionResult> CreateFeedCollection(String name)
        {
            var result = await _feedCollectionRepository.Insert(new FeedCollection() { Name = name, UserId = _userManager.GetUserId(User) });
            return new JsonResult(result.Id);
        }

        [HttpPost("api/feed/addfeed")]
        public async Task<IActionResult> AddFeedToCollection(String feedUri, long collectionId)
        {
            if (String.IsNullOrWhiteSpace(feedUri))
            {
                return BadRequest(nameof(feedUri));
            }
            var collection = await _feedCollectionRepository.GetById(collectionId);
            if(collection==null)
            {
                return BadRequest(nameof(collectionId));
            }
            if (collection.UserId != _userId)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
            feedUri = feedUri.ToLower();
            var feed = await _feedRepository.FirstOrDefault(x => x.Uri == feedUri);
            if (feed == null)
            {
                feed = new Feed()
                {
                    Uri = feedUri,
                    Type = "rss"
                };
                feed = await _feedRepository.Insert(feed);
            }
            var feedCollection = await _feedCollectionsFeedRepository.Insert(
                                              new FeedCollectionsFeed()
                                              {
                                                  Feed = feed,
                                                  FeedCollection = collection
                                              });
            return Ok();
        }

        [HttpGet("api/feed/{id}")]
        public async Task<IActionResult> GetNews(long collectionId)
        {
            var collection = await _feedCollectionRepository.GetById(collectionId);
            if (collection == null)
            {
                return BadRequest(nameof(collectionId));
            }
            if (collection.UserId != _userId)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
            if(collection.FeedCollections.Count==0)
            {
                return NoContent();
            }

            List<ISyndicationFeedOutput> news = new List<ISyndicationFeedOutput>();
            foreach (var item in collection.FeedCollections)
            {
                news.Add(await _cacheManagerInput.GetItemAsync(item.FeedId));
            }
            var a = await _cacheManagerInput.GetItemAsync(1);
            
            return new JsonResult(0);
        }

    }
}