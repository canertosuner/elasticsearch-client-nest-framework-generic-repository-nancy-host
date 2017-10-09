using System;
using System.Configuration;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearchClient.Helper
{
    public static class EsHelper
    {
        public static ElasticClient CreateElasticClient(string indexName)
        {
            var node = new SingleNodeConnectionPool(new Uri(ConfigurationSettings.AppSettings["ElasticSearchURI"]));
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(indexName + "_defaultindex");
            return new ElasticClient(settings);
        }

        public static void CheckIndex<T>(T document, ElasticClient elasticClient, string indexName)
        {
            var response = elasticClient.IndexExists(indexName);
            if (!response.Exists)
            {
                elasticClient.CreateIndex(indexName, index =>
                   index.Mappings(ms =>
                       ms.Map<T>(x => x.AutoMap())));
            }
        }
    }
}
