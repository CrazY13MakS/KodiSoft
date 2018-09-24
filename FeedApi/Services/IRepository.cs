using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Services
{
    public interface IRepository<T>
    {
        Task<T> GetById(long id);
        Task<bool> Insert(T value);
        Task<bool> Delete(long id);
        Task<bool> Update(T value);
        Task<List<T>> GetList(int take, int skip = 0);
    }
}
