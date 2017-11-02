using System;
using System.Configuration;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearchClient
{
    public static class ElasticSearchClientHelper
    {
        public static ElasticClient CreateElasticClient()
        {
            var node = new SingleNodeConnectionPool(new Uri(ConfigurationSettings.AppSettings["ElasticSearchApiAddress"]));
            var settings = new ConnectionSettings(node);
            return new ElasticClient(settings);
        }

        public static void CheckIndex<T>(ElasticClient client, string indexName) where T : class
        {
            var response = client.IndexExists(indexName);
            if (!response.Exists)
            {
                client.CreateIndex(indexName, index =>
                   index.Mappings(ms =>
                       ms.Map<T>(x => x.AutoMap())));
            }
        }
    }
}
