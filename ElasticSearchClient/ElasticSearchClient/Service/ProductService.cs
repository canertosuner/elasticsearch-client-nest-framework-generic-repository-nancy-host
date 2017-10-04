using System;
using ElasticSearchClient.Models;
using ElasticSearchClient.Repository;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Elasticsearch.Net;
using Nest;

namespace ElasticSearchClient.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            var indexName = ConfigurationSettings.AppSettings["ElasticSearchIndexName"];
            var exClient = CreateElasticClient(indexName);

            _productRepository = new ProductRepository(exClient, indexName);
        }

        public List<Product> Search(SearchProductRequest reqModel)
        {
            return _productRepository.Search(reqModel.Query).ToList();
        }

        public Product Save(SaveProductRequest reqModel)
        {
            _productRepository.Save(reqModel.Product);
            return _productRepository.Get(reqModel.Product.Id);
        }

        public Product Update(UpdateProductRequest reqModel)
        {
            _productRepository.Update(reqModel.Product);
            return _productRepository.Get(reqModel.Product.Id);
        }

        public List<Product> GetAll()
        {
            return _productRepository.All().ToList();
        }

        public bool Delete(Guid productId)
        {
            return _productRepository.Delete(productId);
        }

        private static ElasticClient CreateElasticClient(string indexName)
        {
            var node = new SingleNodeConnectionPool(new Uri(ConfigurationSettings.AppSettings["ElasticSearchURI"]));
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(indexName + "_defaultindex");
            return new ElasticClient(settings);
        }
    }
}
