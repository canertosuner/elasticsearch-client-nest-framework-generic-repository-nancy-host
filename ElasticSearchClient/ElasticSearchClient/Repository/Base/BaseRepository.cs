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
                search.MatchAll()).Documents;
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

        public IEnumerable<T> SearchByQuery(string query)
        {
            var result = ElasticClient.Search<T>(s =>
                             s.Query(q =>
                             q.QueryString(qs => qs.Query(query))).Index(IndexName));
            if (!result.IsValid)
            {
                throw new Exception("Search operation is not completed !");
            }
            return result.Documents;
        }

        public abstract IEnumerable<T> Search(T search);
        
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
