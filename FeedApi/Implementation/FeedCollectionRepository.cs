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
    public class FeedCollectionRepository : IRepository<FeedCollection>
    {
        ApplicationDbContext _dbContext;
        public FeedCollectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Delete(long id)
        {
            var entity = await _dbContext.FeedCollections.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _dbContext.FeedCollections.Remove(entity);
                return await _dbContext.SaveChangesAsync() == 1;
            }
            return false;
        }

        public async Task<FeedCollection> FirstOrDefault(Expression<Func<FeedCollection, bool>> func)
        {
            return await _dbContext.FeedCollections.Include("FeedCollections.Feed").FirstOrDefaultAsync(func);
        }

        public async Task<FeedCollection> GetById(long id)
        {
            return await _dbContext.FeedCollections.Include("FeedCollections.Feed").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FeedCollection>> GetList(int take, int skip = 0)
        {
            return await _dbContext.FeedCollections.Include("FeedCollections.Feed").Skip(skip).Take(take).ToListAsync();
        }

        public async Task<FeedCollection> Insert(FeedCollection value)
        {
            var res = await _dbContext.FeedCollections.AddAsync(value);
            return res.Entity;
        }

        public async Task<bool> Update(FeedCollection value)
        {
            _dbContext.FeedCollections.Update(value);
            return await _dbContext.SaveChangesAsync() == 1;
        }

    }
}
