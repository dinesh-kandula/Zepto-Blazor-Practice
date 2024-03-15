using Microsoft.AspNetCore.Components;
using ModelsClassLibrary.Models;

namespace BlazorApp2.Pages
{
    public partial class PizzaHome : ComponentBase
    {
        protected List<PizzaSpecial> Specials = []; 

        protected override async Task OnInitializedAsync()
        {
            Specials = await apiServices.GetApiData<PizzaSpecial>("/zepto/Pizza/GetAllPizzas");

            //Specials.ForEach(special =>
            //{
            //    Console.WriteLine($"Name : {special.Name}, Description: {special.Description}");
            //});
        }
    }
}



//specials.AddRange(new List<PizzaSpecial>
//{
//    new() { Name = "The Baconatorizor", BasePrice =  11.99M, Description = "It has EVERY kind of bacon", ImageUrl="img/pizzas/bacon.jpg"},
//    new() { Name = "Buffalo chicken", BasePrice =  12.75M, Description = "Spicy chicken, hot sauce, and blue cheese, guaranteed to warm you up", ImageUrl="img/pizzas/meaty.jpg"},
//    new() { Name = "Veggie Delight", BasePrice =  11.5M, Description = "It's like salad, but on a pizza", ImageUrl="img/pizzas/salad.jpg"},
//    new() { Name = "Margherita", BasePrice =  9.99M, Description = "Traditional Italian pizza with tomatoes and basil", ImageUrl="img/pizzas/margherita.jpg"},
//    new() { Name = "Basic Cheese Pizza", BasePrice =  11.99M, Description = "It's cheesy and delicious. Why wouldn't you want one?", ImageUrl="img/pizzas/cheese.jpg"},
//    new() { Name = "Classic pepperoni", BasePrice =  10.5M, Description = "It's the pizza you grew up with, but Blazing hot!", ImageUrl="img/pizzas/pepperoni.jpg" }
//});
