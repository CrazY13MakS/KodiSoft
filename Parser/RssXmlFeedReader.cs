using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using Parser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parser
{
   public class RssFeedParser : ParserBase
    {

        readonly ISyndicationFeedParser _parser;

        public RssFeedParser(ISyndicationFeedParser parser = null) : base(new List<SourceType>(new[] { SourceType.Rss }))
        {
            _parser = parser ?? new RssParser();
        }

        public override Task<ISyndicationFeedOutput> ParseAsync(string path)
        {
            return ParseString(path);
        }

        public override bool TryParse(string path, out ISyndicationFeedOutput result)
        {
            result = null;
            try
            {
                result = ParseString(path).Result;
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private async Task<ISyndicationFeedOutput> ParseString(String path)
        {
            var feedResult = new FeedOutput();

            using (var xmlReader = XmlReader.Create(path, new XmlReaderSettings() { Async = true }))
            {
                var feedReader = new Microsoft.SyndicationFeed.Rss.RssFeedReader(xmlReader, _parser);

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Category:
                            feedResult.Categories.Add(await feedReader.ReadCategory());
                            break;

                        case SyndicationElementType.Image:
                            feedResult.Images.Add(await feedReader.ReadImage());
                            break;

                        case SyndicationElementType.Item:
                            feedResult.Items.Add(await feedReader.ReadItem());
                            break;

                        case SyndicationElementType.Link:
                            feedResult.Links.Add(await feedReader.ReadLink());
                            break;

                        case SyndicationElementType.Person:
                            feedResult.Persons.Add(await feedReader.ReadPerson());
                            break;

                        default:
                            feedResult.Content.Add(await feedReader.ReadContent());
                            break;
                    }
                }
            }
            return feedResult;           
        }
    }
}
