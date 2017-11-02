using System;
using ElasticSearchClient.Models;
using ElasticSearchClient.Repository;
using System.Collections.Generic;
using System.Linq;
using ElasticSearchClient.Models.Request;
using ElasticSearchClient.Models.Response;

namespace ElasticSearchClient.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public List<Product> Search(SearchProductRequest reqModel)
        {
            return _productRepository.Search(reqModel).ToList();
        }

        public SaveProductResponse Save(SaveProductRequest reqModel)
        {
            _productRepository.Save(reqModel.Product);
            return new SaveProductResponse { Product = _productRepository.Get(reqModel.Product.Id) };
        }

        public UpdateProductResponse Update(UpdateProductRequest reqModel)
        {
            _productRepository.Update(reqModel.Product);
            return new UpdateProductResponse { Product = _productRepository.Get(reqModel.Product.Id) };
        }

        public List<Product> GetAll()
        {
            return _productRepository.All().ToList();
        }

        public bool Delete(Guid productId)
        {
            return _productRepository.Delete(productId);
        }
    }
}
