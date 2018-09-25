using Parser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Services
{
    public interface ICacheManager<TElem,TId> where TElem:class
    {
         Task<TElem> GetItemAsync(TId id);
    }
}
