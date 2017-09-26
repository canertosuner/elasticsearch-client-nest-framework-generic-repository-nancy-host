using ElasticSearchClient.Models;
using ElasticSearchClient.Service;
using Nancy;
using Nancy.ModelBinding;

namespace ElasticSearchClient.Modules
{
    public class ProductModule : NancyModule
    {
        private readonly IProductService _productService;

        public ProductModule()
        {
            _productService = new ProductService();

            Post["/product/search"] = parameters =>
            {
                var request = this.Bind<SearchProductRequest>();

                return _productService.Search(request);
            };

            Post["/product/save"] = parameters =>
            {
                var request = this.Bind<SaveProductRequest>();

                return _productService.Save(request);
            };

            Put["/product/update"] = parameters =>
            {
                var request = this.Bind<UpdateProductRequest>();

                return _productService.Update(request);
            };

            Get["/product/all"] = parameters =>
            {
                return _productService.GetAll();
            };

            Delete["/product/delete/{productId}"] = parameters =>
            {
                var productId = parameters.productId;
                return _productService.Delete(productId);
            };
        }
    }
}
