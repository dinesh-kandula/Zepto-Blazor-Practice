using BlazorApp2.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using ModelsClassLibrary.Models;
using System;
using System.Net;
using System.Text;
using Telerik.SvgIcons;

namespace BlazorApp2.Pages.ProductComponets
{
    public partial class FruitsVegetables
    {
        public List<Product> TotalProducts = [];
        public List<Product> Products = [];

        private string statusCodeError = "";
        private bool error = false;

        //Filter Properties
        private readonly List<CategoryEnum> receviedCategories = [];
        private readonly List<int> receviedcategoryIntArray = [];
        private string productNameSearch = "";
        private bool SearchLoader = false;

        private readonly int pageSize = 8;
        private int pageIndex = 1;
        private int numberOfPages = 0;


        protected override async Task OnInitializedAsync()
        {
            TotalProducts = await apiServices.GetApiData<Product>($"/zepto/Products/GetAllProducts");
            numberOfPages = (int)Math.Ceiling((double)TotalProducts.Count / pageSize);
            Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        private void OnPageIndexChange(int index)
        {
            pageIndex = index;
            numberOfPages = (int)Math.Ceiling((double)TotalProducts.Count / pageSize);
            Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            StateHasChanged();
        }

        public async Task FilterTheProductsAsync()
        {
            SearchLoader = true;
            string searchQueryParameter = "";
            string categoryQueryParameter = "";

            if (!string.IsNullOrWhiteSpace(productNameSearch))
            {
                var queryStringBuilder = new StringBuilder();
                queryStringBuilder.Append($"productName={productNameSearch}");
                searchQueryParameter = queryStringBuilder.ToString();
            }

            if (receviedcategoryIntArray.Count > 0)
            {
                var queryStringBuilder = new StringBuilder();

                foreach (var category in receviedcategoryIntArray)
                {
                    queryStringBuilder.Append($"categories={category}&");
                }
                queryStringBuilder.Length -= 1;

                categoryQueryParameter = "&" + queryStringBuilder.ToString();
            }

            string url = $"zepto/Products/GetAllProducts?{searchQueryParameter}{categoryQueryParameter}";
            try
            {
                TotalProducts = await apiServices.GetApiData<Product>(url);
                numberOfPages = (int)Math.Ceiling((decimal)TotalProducts.Count / pageSize);
                Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                error = false;
            }
            catch (HttpListenerException ex)
            {
                statusCodeError = ex.Message;
                error = true;
            }

            SearchLoader = false;
            StateHasChanged();
        }

        private async Task OnFilterCategoryAsync(int data)
        {
            if (data >= 0)
            {
                if (!receviedcategoryIntArray.Contains(data))
                {
                    receviedcategoryIntArray.Add(data);
                    receviedCategories.Add((CategoryEnum)data);
                    StateHasChanged();
                    await FilterTheProductsAsync();
                }
            }
        }

        private async Task DeleteChipSetAsync(CategoryEnum category)
        {
            if (receviedCategories.Contains(category))
            {
                receviedCategories.Remove(category);
                receviedcategoryIntArray.Remove((int)category);
                StateHasChanged();
                await FilterTheProductsAsync();
            }
        }

        private async void FilterOnSearchText(string text)
        {
            productNameSearch = string.IsNullOrWhiteSpace(text) ? "" : text;
            StateHasChanged();
            await FilterTheProductsAsync();
        }

        private async void ClearFilters()
        {
            SearchLoader = true;
            receviedCategories.Clear();
            receviedcategoryIntArray.Clear();
            productNameSearch = "";
            TotalProducts = await apiServices.GetApiData<Product>("/zepto/Products/GetAllProducts");
            numberOfPages = (int)Math.Ceiling((decimal)TotalProducts.Count / pageSize);
            Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            SearchLoader = false;
            StateHasChanged();
        }

    }
}
