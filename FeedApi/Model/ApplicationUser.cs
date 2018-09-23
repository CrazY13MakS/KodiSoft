using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Model
{
    public class ApplicationUser:IdentityUser
    {
        public virtual List<FeedCollection> FeedCollections { get; set; }
    }
}
