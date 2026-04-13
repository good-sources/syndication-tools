using System.Collections.Generic;
using System.Linq;
using AggregationServiceClient.Models;

namespace AggregationServiceClient
{
    class OutputFormatter
    {
        public string FormatCollections(IEnumerable<CollectionModel> collections)
        {
            string output = (collections.Count() > 0) ? "" : "No collections\n";

            foreach (CollectionModel collection in collections)
            {
                output = output + collection.Name + "\n";
            }

            return output;
        }

        public string FormatSourceTypesDictionary(IDictionary<string, int> sourceTypes)
        {
            string output = string.Empty;

            foreach (KeyValuePair<string, int> sourceType in sourceTypes)
            {
                output = output + sourceType.Value + " for " + sourceType.Key + ", ";
            }

            if (output.Length > 2)
            {
                output = output.Substring(0, output.Length - 2);
            }

            return output;
        }

        public string FormatContents(IEnumerable<ContentModel> contents, string collectionName)
        {
            string output = (contents.Count() > 0) ? $"Contents for {collectionName} collection:\n\n" : "No contents for the specified collection\n";

            foreach (ContentModel content in contents)
            {
                output = output + $"Title: {content.Title ?? string.Empty}\nDescription: {content.Description ?? string.Empty}\nAuthor: {content.Author ?? string.Empty}\nLink: {content.Link ?? string.Empty}";

                if (content.Published.HasValue)
                {
                    output = output + $"\nPublished: {content.Published}";
                }

                output = output + "\n\n";
            }

            return output;
        }
    }
}
