using Parser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class TypeConverter
    {
        public static SourceType GetSourceType(String type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "rss": return SourceType.Rss;
                case "xml": return SourceType.XML;
                case "atom": return SourceType.Atom;
                case "html": return SourceType.HTML;
                default:
                    return SourceType.Unknown;
            }
        }
        public static OutputType GetOutputType(String type)
        {
            type = type.ToLower();
            switch (type)
            {
                case "rss": return OutputType.Rss;
                case "atom": return OutputType.Atom;
                case "xml": return OutputType.XML;

                default:
                    return OutputType.Rss;
            }
        }
    }
}
