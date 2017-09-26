using ElasticSearchClient.Models;
using ElasticSearchClient.Service;
using Nancy;
using Nancy.ModelBinding;

namespace ElasticSearchClient.Modules
{
    public class ProductModule : NancyModule
    {
        public ProductModule()
        {
            Post["/product/search"] = parameters =>
            {
                var request = this.Bind<SearchProductRequest>();
                var productService = new ProductService();

                return productService.Search(request);
            };

            Post["/product/save"] = parameters =>
            {
                var request = this.Bind<SaveProductRequest>();
                var productService = new ProductService();

                return productService.Save(request);
            };

            Put["/product/update"] = parameters =>
            {
                var request = this.Bind<UpdateProductRequest>();
                var productService = new ProductService();

                return productService.Update(request);
            };
        }
    }
}
