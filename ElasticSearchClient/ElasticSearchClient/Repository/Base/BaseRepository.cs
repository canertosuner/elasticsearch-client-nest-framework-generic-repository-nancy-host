using ElasticSearchClient.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Elasticsearch.Net;

namespace ElasticSearchClient.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ElasticClient ElasticClient;
        protected readonly string IndexName;

        protected BaseRepository(string indexName)
        {
            ElasticClient = CreateElasticClient();
            IndexName = indexName;
        }

        public IEnumerable<T> All()
        {
            return ElasticClient.Search<T>(search =>
                search.MatchAll().Index(IndexName)).Documents;
        }

        public bool Delete(Guid id)
        {
            var result = ElasticClient.Delete<T>(id.ToString(), idx => idx.Index(IndexName));
            if (!result.Found)
            {
                throw new Exception("Delete operation is not completed for the product : " + id);
            }
            return result.Found;
        }

        public T Get(Guid id)
        {
            var result = ElasticClient.Get<T>(id.ToString(), idx => idx.Index(IndexName));
            if (!result.Found)
            {
                throw new Exception("Get operation is not completed !");
            }
            return result.Source;
        }

        public void Save(T entity)
        {
            CheckIndex();

            entity.Id = Guid.NewGuid();
            var result = ElasticClient.Index(entity, idx => idx.Index(IndexName));
            if (!result.IsValid)
            {
                throw new Exception("Save operation is not completed !");
            }
        }

        public void Update(T entity)
        {
            var result = ElasticClient.Update(
                    new DocumentPath<T>(entity), u =>
                        u.Doc(entity).Index(IndexName));
            if (!result.IsValid)
            {
                throw new Exception("Update operation is not completed !");
            }
        }

        public IEnumerable<T> Search(BaseSearchModel request)
        {
            var fields = new List<QueryContainer>();
            foreach (var item in request.Fields)
            {
                fields.Add(new TermQuery
                {
                    Field = item.Key,
                    Value = item.Value
                });
            }

            var queryContainer = new QueryContainer(new BoolQuery
            {
                Must = fields
            });

            var response = ElasticClient.Search<T>(new SearchRequest(IndexName, typeof(T))
            {
                Size = request.Size,
                From = request.From,
                Query = (QueryContainer)queryContainer
            });

            if (!response.IsValid)
            {
                throw new Exception("Search operation is not completed !");
            }
            return response.Documents;
        }
        
        private void CheckIndex()
        {
            var response = ElasticClient.IndexExists(IndexName);
            if (!response.Exists)
            {
                ElasticClient.CreateIndex(IndexName, index =>
                   index.Mappings(ms =>
                       ms.Map<T>(x => x.AutoMap())));
            }
        }

        private ElasticClient CreateElasticClient()
        {
            var node = new SingleNodeConnectionPool(new Uri(ConfigurationSettings.AppSettings["ElasticSearchApiAddress"]));
            var settings = new ConnectionSettings(node);
            return new ElasticClient(settings);
        }
    }
}
