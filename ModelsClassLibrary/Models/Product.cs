using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models
{
    public enum CategoryEnum
    {
        FruitsAndVegetables,
        AttaRiceOilAndDals,
        MasalaAndDryFruits,
        SweetCravings,
        FrozenFoodAndIceCreams,
        PackageFood,
        DairyBreadAndEggs,
        ColdDrinksAndJuices,
        MeatFishAndEggs
    }

    public partial class Product
    {
        public int Id { get; set; }

        [Required]
        public required string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The price must be at least {1}.")]
        public decimal BasePrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "No Negative Discounts.")]
        public decimal Offer { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public CategoryEnum Category { get; set; }  

        public string GetFormattedBasePrice() => BasePrice.ToString("0.00");

        public string GetDiscountedPrice() {
            decimal discountedPrice = BasePrice * (Offer / 100);
            decimal discountPrice = BasePrice - discountedPrice;
            return discountPrice.ToString("0.00");
        }

        public string GetOfferPercentage() => Offer.ToString("0")+ "% Off";

        public string GetCategoryDisplayName()
        {
            string catString = Category.ToString();
            return ConvertCategory(catString);
        }
     
        public string ConvertCategory(string catString)
        {
            StringBuilder result = new();
            for (int i = 0; i < catString.Length; i++)
            {
                char currentChar = catString[i];
                if (char.IsUpper(currentChar) && i > 0)
                {
                    result.Append(' ');
                }
                if (i + 2 < catString.Length && catString.Substring(i, 3) == "And")
                {
                    result.Append("&");
                    i += 2;
                }
                else
                {
                    result.Append(currentChar);
                }
            }
            return result.ToString();
        }
    }
}
