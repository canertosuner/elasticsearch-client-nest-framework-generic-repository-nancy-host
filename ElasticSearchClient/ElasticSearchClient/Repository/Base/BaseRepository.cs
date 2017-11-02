using ElasticSearchClient.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using Elasticsearch.Net;

namespace ElasticSearchClient.Repository.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ElasticClient _elasticClient;
        private readonly string _indexName;

        protected BaseRepository(string indexName)
        {
            _elasticClient = CreateElasticClient();
            _indexName = indexName;
        }

        public IEnumerable<T> All()
        {
            return _elasticClient.Search<T>(search =>
                search.MatchAll().Index(_indexName)).Documents;
        }

        public bool Delete(Guid id)
        {
            var result = _elasticClient.Delete<T>(id.ToString(), idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return result.Found;
        }

        public T Get(Guid id)
        {
            var result = _elasticClient.Get<T>(id.ToString(), idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return result.Source;
        }

        public void Save(T entity)
        {
            CheckIndex();

            entity.Id = Guid.NewGuid();
            var result = _elasticClient.Index(entity, idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
        }

        public void Update(T entity)
        {
            var result = _elasticClient.Update(
                    new DocumentPath<T>(entity), u =>
                        u.Doc(entity).Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
        }

        public IEnumerable<T> Search(BaseSearchModel request)
        {
            var dynamicQuery = new List<QueryContainer>();
            foreach (var item in request.Fields)
            {
                dynamicQuery.Add(Query<T>.Match(m => m.Field(new Field(item.Key.ToLower())).Query(item.Value)));
            }
            
            var result = _elasticClient.Search<T>(s => s
                                       .From(request.From)
                                       .Size(request.Size)
                                       .Index(_indexName)
                                        .Query(q => q.Bool(b => b.Must(dynamicQuery.ToArray()))));

            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }

            return result.Documents;
        }

        private void CheckIndex()
        {
            var response = _elasticClient.IndexExists(_indexName);
            if (!response.Exists)
            {
                _elasticClient.CreateIndex(_indexName, index =>
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
