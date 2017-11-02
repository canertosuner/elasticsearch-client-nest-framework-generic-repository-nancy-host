using System;
using System.Collections.Generic;
using ElasticSearchClient.Models;
using ElasticSearchClient.Models.Request;
using ElasticSearchClient.Models.Response;

namespace ElasticSearchClient.Service
{
    public interface IProductService
    {
        List<Product> Search(SearchProductRequest reqModel);
        SaveProductResponse Save(SaveProductRequest reqModel);
        UpdateProductResponse Update(UpdateProductRequest reqModel);
        List<Product> GetAll();
        bool Delete(Guid productId);
    }
}
