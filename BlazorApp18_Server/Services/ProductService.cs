using BlazorApp18_Server.Utilities;
using Microsoft.IdentityModel.Tokens;
using ModelsClassLibrary.Models;

namespace BlazorApp18_Server.Services
{
    public class ProductService(ApiServices apiServices)
    {
        public async Task<List<Product>> GetFilteredProductsAsync(string? productName=null, string? category=null)
        {
            var queryBuilder = new QueryParameterBuilder();
            if (!string.IsNullOrEmpty(productName))
                queryBuilder.Add("productName", productName);

            if (!string.IsNullOrEmpty(category))
                queryBuilder.Add("category", category);

            string queryString = queryBuilder.Build();
            string url = $"zepto/Products/GetAllProducts?{queryString}";

            return await apiServices.GetApiData<Product>(url);
        }
    }
}
