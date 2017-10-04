﻿using ElasticSearchClient.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ElasticSearchClient.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ElasticClient _elasticClient;
        private readonly string _indexName;

        public BaseRepository(ElasticClient client, string indexName)
        {
            _elasticClient = client;
            _indexName = indexName;
        }

        public IEnumerable<T> All()
        {
            return _elasticClient.Search<T>(search =>
                search.MatchAll()).Documents;
        }

        public bool Delete(Guid id)
        {
            var result = _elasticClient.Delete<T>(id.ToString(), idx => idx.Index(_indexName));
            if (!result.Found)
            {
                throw new Exception("Delete operation is not completed for the product : " + id);
            }
            return result.Found;
        }

        public T Get(Guid id)
        {
            var result = _elasticClient.Get<T>(id.ToString(), idx => idx.Index(_indexName));
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
            var result = _elasticClient.Index(entity, idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception("Save operation is not completed !");
            }
        }

        public void Update(T entity)
        {
            var result = _elasticClient.Update(
                    new DocumentPath<T>(entity), u =>
                        u.Doc(entity));
            if (!result.IsValid)
            {
                throw new Exception("Update operation is not completed !");
            }
        }

        public IEnumerable<T> Search(string search)
        {
            var result = _elasticClient.Search<T>(s =>
                                    s.Query(query =>
                                    query.QueryString(qs => qs.Query(search))).Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception("Search operation is not completed !");
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
    }
}
