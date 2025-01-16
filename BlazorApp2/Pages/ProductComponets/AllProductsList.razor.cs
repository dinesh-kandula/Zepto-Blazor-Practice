using BlazorApp2.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Models.Enums;
using System;
using System.Net;
using System.Text;

namespace BlazorApp2.Pages.ProductComponets
{
    public partial class AllProductsList
    {
        public List<Product> TotalProducts = []; //Total List
        public List<Product> Products = []; //Pagination Show List

        private string statusCodeError = "";
        private bool error = false;

        //Filter Properties
        public ICollection<object>? SelectedCategories { get; set; }
        private string productNameSearch = "";

        //Loader
        private bool SearchLoader = false;

        //Paginantion
        private readonly int pageSize = 8;
        private int pageIndex = 1;
        private int numberOfPages = 0;

        protected override async Task OnInitializedAsync()
        {
            TotalProducts = await apiServices.GetApiData<Product>($"/zepto/Products/GetAllProducts");
            FilterThePagination();
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

            if (SelectedCategories!=null && SelectedCategories.Count > 0)
            {
                var queryStringBuilder = new StringBuilder();

                foreach (var category in SelectedCategories)
                {
                    queryStringBuilder.Append($"categories={(int)category}&");
                }
                categoryQueryParameter = queryStringBuilder.ToString();
            }

            string url = $"zepto/Products/GetAllProducts?{searchQueryParameter}&{categoryQueryParameter}";
            Console.WriteLine(url);
            try
            {
                TotalProducts = await apiServices.GetApiData<Product>(url);
                FilterThePagination();
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

        private async void OnFilterCategoryAsync(ICollection<object> categoryCollection)
        {
            SelectedCategories = categoryCollection.ToList();
            StateHasChanged();
            await FilterTheProductsAsync();
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
            productNameSearch = "";
            SelectedCategories = null;
            TotalProducts = await apiServices.GetApiData<Product>("/zepto/Products/GetAllProducts");
            FilterThePagination();
            SearchLoader = false;
            StateHasChanged();
        }
        private void OnPageIndexChange(int index)
        {
            pageIndex = index;
            FilterThePagination();
            StateHasChanged();
        }

        private void FilterThePagination()
        {
            numberOfPages = (int)Math.Ceiling((double)TotalProducts.Count / pageSize);
            Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }
    }
}
