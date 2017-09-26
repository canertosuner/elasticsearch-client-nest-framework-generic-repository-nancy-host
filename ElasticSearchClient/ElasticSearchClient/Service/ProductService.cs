using ElasticSearchClient.Models;
using ElasticSearchClient.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchClient.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public List<Product> Search(SearchProductRequest reqModel)
        {
            return _productRepository.Search(reqModel.Query).ToList();
        }

        public Product Save(SaveProductRequest reqModel)
        {
            var id = _productRepository.Save(reqModel.Product);
            return _productRepository.Get(reqModel.Product.Id);
        }

        public Product Update(UpdateProductRequest reqModel)
        {
            _productRepository.Update(reqModel.Product);
            return _productRepository.Get(reqModel.Product.Id);
        }
    }
}
