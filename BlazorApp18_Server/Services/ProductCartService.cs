using ModelsClassLibrary.Models;

namespace BlazorApp18_Server.Services
{
    public class ProductCartService
    {
        private List<CartItem> cartItems = new List<CartItem>();

        public List<CartItem> CartItems => cartItems;

        /// <summary>
        /// Add the Product to the cart, the default quantity is 1
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = cartItems.FirstOrDefault(ci => ci.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Total = existingItem.Quantity * decimal.Parse(product.GetDiscountedPrice());
            }
            else
            {
                cartItems.Add(new CartItem(product, quantity));
            }
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        /// <param name="product"></param>
        public void RemoveFromCart(CartItem product)
        {
            var existingItem = cartItems.FirstOrDefault(ci => ci.Product.Id == product.Product.Id);
            if (existingItem != null)
            {
                cartItems.Remove(existingItem);
            }
        }

        /// <summary>
        /// Increase the quantity of a product in the cart
        /// </summary>
        /// <param name="product"></param>
        public void IncreaseQuantity(CartItem product)
        {
            var existingItem = cartItems.FirstOrDefault(ci => ci.Product.Id == product.Product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.Total = existingItem.Quantity * decimal.Parse(product.Product.GetDiscountedPrice());
            }
        }

        /// <summary>
        /// Decrease the quantity of a product in the cart
        /// </summary>
        /// <param name="product"></param>
        public void DecreaseQuantity(CartItem product)
        {
            var existingItem = cartItems.FirstOrDefault(ci => ci.Product.Id == product.Product.Id);
            if (existingItem != null && existingItem.Quantity > 1)
            {
                existingItem.Quantity--;
                existingItem.Total = existingItem.Quantity * decimal.Parse(product.Product.GetDiscountedPrice());
            }
        }

        /// <summary>
        /// Clear all items from the cart
        /// </summary>
        public void ClearCart()
        {
            cartItems.Clear();
        }

        /// <summary>
        /// Calculate total value of the cart
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalCartValue()
        {
            return cartItems.Sum(ci => ci.Total);
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Total = quantity * decimal.Parse(product.GetDiscountedPrice());
        }
    }
}
