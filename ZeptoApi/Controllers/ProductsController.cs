using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Repository;
using System.Data;
using System.Linq;

namespace ZeptoApi.Controllers
{
    [Route("zepto/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //[Route("GetAllProducts")]
        //public async Task<ActionResult> GetAllProducts(string? productName, [FromQuery] List<int>? categories)
        //{
        //    try
        //    {
        //        List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync();

        //        if (productName != null && !string.IsNullOrWhiteSpace(productName))
        //        {
        //            List<Product> filteredProducts = products
        //                   .Where(p => p.ProductName.Contains(productName, StringComparison.CurrentCultureIgnoreCase))
        //                   .ToList();
        //            products = filteredProducts;
        //            ;
        //        }

        //        if (categories != null && categories.Count > 0)
        //        {
        //            List<Product> filteredProducts = products
        //            .Where(p => categories.Contains((int)p.Category))
        //            .ToList();
        //            products = filteredProducts;
        //        }

        //        products = [.. products.OrderBy(p => p.ProductName)];

        //        return Ok(products);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error retrieving data from the database");
        //    }
        //}

        [HttpGet]
        [Route("GetAllProducts")]
        public ActionResult GetAllProducts(string? productName, [FromQuery] List<int>? categories)
        {
            try
            {
                SqlParameter productParam = null!;
                if (productName != null)
                {
                    productParam = new SqlParameter("@productName", productName);
                }

                List<Product> products = _unitOfWork.ProductRepository.GetAllProductsWithFilter(productParam);

                if (categories != null && categories.Count > 0)
                {
                    List<Product> filteredProducts = products
                    .Where(p => categories.Contains((int) p.Category))
                    .ToList();
                    products = filteredProducts;
                }

                if(products.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "No Product with applied Filter, Try Again");
                }

                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        [HttpGet]
        [Route("GetProductById/{Id}")]
        public async Task<ActionResult> GetProductById(int Id)
        {
            try
            {
                var productDetails = await _unitOfWork.ProductRepository.GetAsync(Id);
                if (productDetails == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                    $"No Product Found with given Id: {Id}");
                }
                return Ok(productDetails);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            try
            {
                Product newlyAddedProduct = await _unitOfWork.ProductRepository.AddEntity(product);
                await _unitOfWork.CompleteAsync();
                return StatusCode(StatusCodes.Status201Created, newlyAddedProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Posting data to the database");
            }
        }
    }
}
