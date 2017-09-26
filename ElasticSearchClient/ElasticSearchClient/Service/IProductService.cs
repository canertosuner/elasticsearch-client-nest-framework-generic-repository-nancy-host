using System;
using System.Collections.Generic;
using ElasticSearchClient.Models;

namespace ElasticSearchClient.Service
{
    public interface IProductService
    {
        List<Product> Search(SearchProductRequest reqModel);
        Product Save(SaveProductRequest reqModel);
        Product Update(UpdateProductRequest reqModel);
        List<Product> GetAll();
        bool Delete(Guid productId);
    }
}
