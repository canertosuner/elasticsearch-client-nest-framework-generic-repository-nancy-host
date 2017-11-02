using System.Collections.Generic;

namespace ElasticSearchClient.Models
{
    public class BaseSearchModel
    {
        public int Size { get; set; }
        public int From { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}
