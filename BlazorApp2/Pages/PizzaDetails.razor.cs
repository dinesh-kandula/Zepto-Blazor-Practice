using Microsoft.AspNetCore.Components;
using ModelsClassLibrary.Models;

namespace BlazorApp2.Pages
{
    public partial class PizzaDetails
    {
        [Parameter]
        public int PizzaId { get; set; }

        protected PizzaSpecial? Pizza { get; set;}

        protected override async Task OnParametersSetAsync()
        {
            Pizza = await apiServices.GetAsync<PizzaSpecial>($"/zepto/Pizza/GetPizzaById/{PizzaId}");
        }
    }
}
