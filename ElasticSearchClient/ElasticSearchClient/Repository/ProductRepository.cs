using ElasticSearchClient.Models;
using ElasticSearchClient.Repository.Base;
using System.Configuration;

namespace ElasticSearchClient.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository() : base(ConfigurationSettings.AppSettings["ProductIndexName"])
        {
        }
    }
}
