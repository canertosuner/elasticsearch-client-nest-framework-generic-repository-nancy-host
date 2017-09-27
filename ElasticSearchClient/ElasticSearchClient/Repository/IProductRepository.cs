using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticSearchClient.Models;
using ElasticSearchClient.Repository.Base;

namespace ElasticSearchClient.Repository
{
    public interface IProductRepository: IBaseRepository<Product>
    {
    }
}
