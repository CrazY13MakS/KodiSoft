using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Model
{
   public interface ISourceTypeParser
    {
        SourceType GetSourceType(String uri);      

    }
}
