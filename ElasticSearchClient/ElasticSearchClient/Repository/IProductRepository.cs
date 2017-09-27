using ElasticSearchClient.Models;
using ElasticSearchClient.Repository.Base;

namespace ElasticSearchClient.Repository
{
    public interface IProductRepository: IBaseRepository<Product>
    {
    }
}
