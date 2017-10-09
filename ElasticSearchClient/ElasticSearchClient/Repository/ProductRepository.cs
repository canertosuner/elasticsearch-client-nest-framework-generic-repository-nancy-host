using System;
using ElasticSearchClient.Models;
using ElasticSearchClient.Repository.Base;
using System.Configuration;
using System.Collections.Generic;

namespace ElasticSearchClient.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository() : base(ConfigurationSettings.AppSettings["ProductIndexName"])
        { }

        public override IEnumerable<Product> Search(Product search)
        {
            var result = ElasticClient.Search<Product>(s =>
                s.Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                                .Match(m => m
                                    .Field(f => f.Name)
                                    .Query(search.Name)
                                )
                        ))).Index(IndexName));

            if (!result.IsValid)
            {
                throw new Exception("Search operation is not completed !");
            }
            return result.Documents;
        }
    }
}
