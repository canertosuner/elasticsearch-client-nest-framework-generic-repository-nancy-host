using ElasticSearchClient.Models;
using ElasticSearchClient.Repository.Base;
using System.Configuration;
using Nest;

namespace ElasticSearchClient.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ElasticClient client, string indexName) : base(client, indexName)
        {
        }
    }
}
