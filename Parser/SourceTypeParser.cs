using Parser.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parser
{
    public class SourceTypeParser : ISourceTypeParser
    {
        public SourceType GetSourceType(string uri)
        {
            if(String.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }
            XDocument xDocument = XDocument.Load(uri);
           
            foreach (XNode node in xDocument.DescendantNodes())
            {
                if (node is XElement)
                {
                    XElement element = (XElement)node;
                    if (element.Name.LocalName.Equals("rss"))
                        return SourceType.Rss;
                    else if(element.Name.LocalName.Equals("feed"))
                    {
                        return SourceType.Atom;
                    }
                }
            }
            return SourceType.Unknown;
        }
       
    }
}
