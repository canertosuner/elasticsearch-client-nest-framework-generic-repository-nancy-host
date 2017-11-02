using ElasticSearchClient.Models.Request;
using ElasticSearchClient.Service;
using Nancy;
using Nancy.ModelBinding;

namespace ElasticSearchClient.Modules
{
    public class ProductModule : NancyModule
    {
        public ProductModule()
        {
            IProductService productService = new ProductService();

            Post["/product/search"] = parameters =>
            {
                var request = this.Bind<SearchProductRequest>();

                return productService.Search(request);
            };

            Post["/product/save"] = parameters =>
            {
                var request = this.Bind<SaveProductRequest>();

                return productService.Save(request);
            };

            Put["/product/update"] = parameters =>
            {
                var request = this.Bind<UpdateProductRequest>();

                return productService.Update(request);
            };

            Get["/product/all"] = parameters =>
            {
                return productService.GetAll();
            };

            Delete["/product/delete/{productId}"] = parameters =>
            {
                var productId = parameters.productId;
                return productService.Delete(productId);
            };
        }
    }
}
