using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using Parser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Parser
{
    public class FeedToRssStringWriter : IFeedWriter
    {

        public Task<string> Write(ISyndicationFeedOutput syndicationFeed)
        {
            var sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var writer = new RssFeedWriter(xmlWriter);
                syndicationFeed.Categories.ToList().ForEach(async x => await writer.Write(x));
                syndicationFeed.Persons.ToList().ForEach(async x => await writer.Write(x));
                syndicationFeed.Content.ToList().ForEach(async x => await writer.Write(x));
                syndicationFeed.Links.ToList().ForEach(async x => await writer.Write(x));
                syndicationFeed.Images.ToList().ForEach(async x => await writer.Write(x));
                syndicationFeed.Items.ToList().ForEach(async x => await writer.Write(x));
                xmlWriter.Flush();
            }
            return Task.FromResult(sw.ToString());
        }

        public Task<string> WriteCollection(IEnumerable<ISyndicationFeedOutput> collection)
        {
            var sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var writer = new RssFeedWriter(xmlWriter);
                AtomFormatter atomFormatter = new AtomFormatter();

                foreach (var syndicationFeed in collection)
                {
                    if (syndicationFeed.SourceType == SourceType.Atom)
                    {
                        syndicationFeed.Categories.ToList().ForEach(async x => await writer.WriteRaw(atomFormatter.Format(x)));
                        syndicationFeed.Persons.ToList().ForEach(async x => await writer.WriteRaw(atomFormatter.Format(x)));
                        syndicationFeed.Content.ToList().ForEach(async x => await writer.WriteRaw(atomFormatter.Format(x)));
                        syndicationFeed.Links.ToList().ForEach(async x => await writer.WriteRaw(atomFormatter.Format(x)));
                        syndicationFeed.Images.ToList().ForEach(async x => await writer.Write(atomFormatter.CreateContent(x)));
                        syndicationFeed.Items.ToList().ForEach(async x => await writer.WriteRaw(atomFormatter.Format(x)));
                    }
                    else
                    {
                        syndicationFeed.Categories.ToList().ForEach(async x => await writer.Write(x));
                        syndicationFeed.Persons.ToList().ForEach(async x => await writer.Write(x));
                        syndicationFeed.Content.ToList().ForEach(async x => await writer.Write(x));
                        syndicationFeed.Links.ToList().ForEach(async x => await writer.Write(x));
                        syndicationFeed.Images.ToList().ForEach(async x => await writer.Write(x));
                        syndicationFeed.Items.ToList().ForEach(async x => await writer.Write(x));
                    }
                    writer.Flush();
                    xmlWriter.Flush();
                }
            }
            return Task.FromResult(sw.ToString());
        }
    }
}
