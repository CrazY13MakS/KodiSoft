using FeedApi.Data;
using FeedApi.Model;
using FeedApi.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FeedApi.Implementation
{
    public class FeedCollectionFeedRepository : IRepository<FeedCollectionsFeed>
    {
        ApplicationDbContext _dbContext;
        public FeedCollectionFeedRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Delete(long id)
        {
            var entity = await _dbContext.FeedCollectionsFeeds.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _dbContext.FeedCollectionsFeeds.Remove(entity);
                return await _dbContext.SaveChangesAsync() == 1;
            }
            return false;
        }


        public async Task<FeedCollectionsFeed> FirstOrDefault(Expression<Func<FeedCollectionsFeed, bool>> func)
        {
            return await _dbContext.FeedCollectionsFeeds
                .Include(x => x.Feed)
                .Include(x => x.FeedCollection)
                .FirstOrDefaultAsync(func);
        }

        public async Task<FeedCollectionsFeed> GetById(long id)
        {
            return await _dbContext.FeedCollectionsFeeds
                .Include(x => x.Feed)
                .Include(x => x.FeedCollection)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FeedCollectionsFeed>> GetList(int take, int skip = 0)
        {
            return await _dbContext.FeedCollectionsFeeds
                                    .Include(x => x.Feed)
                                    .Include(x => x.FeedCollection)
                                    .Skip(skip)
                                    .Take(take)
                                    .ToListAsync();
        }

        public async Task<FeedCollectionsFeed> Insert(FeedCollectionsFeed value)
        {
            var res = await _dbContext.FeedCollectionsFeeds.AddAsync(value);
            return res.Entity;
        }

        public async Task<bool> Update(FeedCollectionsFeed value)
        {
            _dbContext.FeedCollectionsFeeds.Update(value);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}
