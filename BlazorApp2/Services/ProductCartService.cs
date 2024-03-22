using ModelsClassLibrary.Models;

namespace BlazorApp2.Services
{
    public class ProductCartService
    {

        private List<Product> cartItems = [];

        public List<Product> CartItems => cartItems;

        public void AddToCart(Product product)
        {
            cartItems.Add(product);
        }

        public void RemoveFromCart(Product product)
        {
            cartItems.Remove(product);
        }
    }
}
