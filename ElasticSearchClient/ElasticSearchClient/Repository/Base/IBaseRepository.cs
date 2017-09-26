using System;
using System.Collections.Generic;

namespace ElasticSearchClient.Repository.Base
{
    public interface IBaseRepository<T> where T : class
    {
        string Save(T entity);
        T Get(Guid id);
        void Update(T entity);
        bool Delete(Guid id);
        IEnumerable<T> All();
        IEnumerable<T> Search(string query);
    }
}