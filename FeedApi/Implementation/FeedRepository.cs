using FeedApi.Data;
using FeedApi.Model;
using FeedApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FeedApi.Implementation
{
    public class FeedRepository : IRepository<Feed>
    {
        private readonly ApplicationDbContext _dbContext;
        public FeedRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Delete(long id)
        {
            var entity = await _dbContext.Feeds.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _dbContext.Feeds.Remove(entity);
                return await _dbContext.SaveChangesAsync() == 1;
            }
            return false;
        }

        public async Task<Feed> FirstOrDefault(Expression<Func<Feed, bool>> func)
        {
            return await _dbContext.Feeds.FirstOrDefaultAsync(func);
        }

        public async Task<Feed> GetById(long id)
        {
            return await _dbContext.Feeds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Feed>> GetList(int take, int skip = 0)
        {
            return await _dbContext.Feeds.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<Feed> Insert(Feed value)
        {
            var res = await _dbContext.Feeds.AddAsync(value);
            await _dbContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> Update(Feed value)
        {
            _dbContext.Feeds.Update(value);
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}
