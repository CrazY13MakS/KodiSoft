using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Services
{
   public interface ITokenService
    {
        String GenerateToken(String userName, TimeSpan lifeTime);
    }
}
