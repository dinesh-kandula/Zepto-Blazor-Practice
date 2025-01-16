using BlazorApp18_Server.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.IdentityModel.Tokens;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Models.Enums;
using System;
using System.Net;
using System.Text;

namespace BlazorApp18_Server.Components.Pages.ProductComponets
{
    /// <summary>
    /// This class is used to display all the products with pagination, filtering, and search functionality.
    /// </summary>
    public partial class AllProductsList
    {
        /// <summary>
        /// List of all products.
        /// </summary>
        public List<Product> TotalProducts = []; //Total List

        /// <summary>
        /// List of products to be displayed on the current page.
        /// </summary>
        public List<Product> Products = []; //Pagination Show List

        private string statusCodeError = "";
        private bool error = false;

        // Filter Properties
        /// <summary>
        /// Selected category for filtering products.
        /// </summary>
        public string? SelectedCategory { get; set; }

        /// <summary>
        /// Search text for filtering products by name.
        /// </summary>
        private string? ProductNameSearch { get; set; }

        // Loader
        private bool SearchLoader = false;

        // Pagination
        private readonly int pageSize = 9;
        private int pageIndex = 1;
        private int numberOfPages = 0;

        /// <summary>
        /// Initializes the component and loads the initial list of products.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            TotalProducts = await productService.GetFilteredProductsAsync();
            FilterThePagination();
        }

        /// <summary>
        /// Filters the products based on the selected category and search text.
        /// </summary>
        public async Task FilterTheProductsAsync()
        {
            SearchLoader = true;
            try
            {
                TotalProducts = await productService.GetFilteredProductsAsync(ProductNameSearch, SelectedCategory);
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

        /// <summary>
        /// Filters the products based on the selected category.
        /// </summary>
        /// <param name="category">The selected category.</param>
        private async void OnFilterCategoryAsync(string category)
        {
            SelectedCategory = category;
            StateHasChanged();
            await FilterTheProductsAsync();
        }

        /// <summary>
        /// Filters the products based on the search text.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async void FilterOnSearchText(string text)
        {
            ProductNameSearch = string.IsNullOrWhiteSpace(text) ? "" : text;
            StateHasChanged();
            await FilterTheProductsAsync();
        }

        /// <summary>
        /// Clears all filters and reloads the initial list of products.
        /// </summary>
        private async void ClearFilters()
        {
            SearchLoader = true;
            ProductNameSearch = "";
            SelectedCategory = null;
            TotalProducts = await productService.GetFilteredProductsAsync();
            FilterThePagination();
            SearchLoader = false;
            StateHasChanged();
        }

        /// <summary>
        /// Changes the current page index and updates the displayed products.
        /// </summary>
        /// <param name="index">The new page index.</param>
        private void OnPageIndexChange(int index)
        {
            pageIndex = index;
            FilterThePagination();
            StateHasChanged();
        }

        /// <summary>
        /// Filters the products for the current page based on the page index and page size.
        /// </summary>
        private void FilterThePagination()
        {
            numberOfPages = (int)Math.Ceiling((double)TotalProducts.Count / pageSize);
            Products = TotalProducts.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }
    }
}
