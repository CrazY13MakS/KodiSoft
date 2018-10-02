using FeedApi.Model;
using FeedApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IRepository<Feed> _feedRepository;
        private readonly IRepository<FeedCollection> _feedCollectionRepository;
        private readonly IRepository<FeedCollectionsFeed> _feedCollectionsFeedRepository;
        private readonly ICacheManager<ISyndicationFeedOutput, long> _cacheManagerInput;
        private readonly UserManager<ApplicationUser> _userManager;
        IFeedWriterFactory _writerFactory;
        ISourceTypeParser _sourceTypeParser;

        public FeedController(
            IRepository<Feed> feedRepository,
            IRepository<FeedCollection> feedCollectionRepository,
            IRepository<FeedCollectionsFeed> feedCollectionsFeedRepository,
            ICacheManager<ISyndicationFeedOutput, long> cacheManagerInput,
             UserManager<ApplicationUser> userManager,
             IFeedWriterFactory writerFactory,
             ISourceTypeParser sourceTypeParser)
        {
            _cacheManagerInput = cacheManagerInput;
            _feedCollectionRepository = feedCollectionRepository;
            _feedRepository = feedRepository;
            _feedCollectionsFeedRepository = feedCollectionsFeedRepository;
            _userManager = userManager;
            _writerFactory = writerFactory;
            _sourceTypeParser = sourceTypeParser;
        }



        [HttpPost("/createcollection")]
        public async Task<IActionResult> CreateFeedCollection([FromBody] String name)
        {
            var result = await _feedCollectionRepository.Insert(new FeedCollection() { Name = name, UserId = _userManager.GetUserId(User) });
            return new JsonResult(result.Id);
        }

        [HttpPost("/addfeed")]
        public async Task<IActionResult> AddFeedToCollection(AddFeedModel model)
        {
            if (String.IsNullOrWhiteSpace(model.Uri))
            {
                return BadRequest(nameof(model.Uri));
            }
            var collection = await _feedCollectionRepository.GetById(model.Id);
            if (collection == null)
            {
                return BadRequest(nameof(model.Id));
            }
            if (collection.UserId != _userManager.GetUserId(User))
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
            model.Uri = model.Uri.ToLower();
            var feed = await _feedRepository.FirstOrDefault(x => x.Uri == model.Uri);
            if (feed == null)
            {
                feed = new Feed()
                {
                    Uri = model.Uri,
                    Type = _sourceTypeParser.GetSourceType(model.Uri).ToString()
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
        [Route("/news/{id}")]
       [HttpGet]
        public async Task<IActionResult> GetNews(long id)
        {
            var collection = await _feedCollectionRepository.GetById(id);
            if (collection == null)
            {
                return BadRequest(nameof(id));
            }
            if (collection.UserId != _userManager.GetUserId(User))
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
            if (collection.FeedCollections.Count == 0)
            {
                return NoContent();
            }

            List<ISyndicationFeedOutput> news = new List<ISyndicationFeedOutput>();
            foreach (var item in collection.FeedCollections)
            {
                news.Add(await _cacheManagerInput.GetItemAsync(item.FeedId));
            }
            var writer = _writerFactory.FeedWriter(OutputType.Rss);
            var response = await writer.WriteCollection(news);
            return Ok(response);
        }
    }
}