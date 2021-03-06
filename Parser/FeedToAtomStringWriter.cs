﻿using Microsoft.SyndicationFeed.Atom;
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
    public class FeedToAtomStringWriter : IFeedWriter
    {
        public Task<string> Write(ISyndicationFeedOutput syndicationFeed)
        {
            var sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var writer = new AtomFeedWriter(xmlWriter);
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
            throw new NotImplementedException();
        }
    }
}
