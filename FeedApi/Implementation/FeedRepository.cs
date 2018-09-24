using FeedApi.Data;
using FeedApi.Model;
using FeedApi.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Implementation
{
    public class FeedRepository : IRepository<Feed>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _dbContext;
        public FeedRepository(IMemoryCache cache, ApplicationDbContext dbContext)
        {
            _memoryCache = cache;
            _dbContext = dbContext;
        }

        public Task<bool> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Feed> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Feed>> GetList(int take, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(Feed value)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Feed value)
        {
            throw new NotImplementedException();
        }
    }
}
