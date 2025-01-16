using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Models.Enums;
using System.Data;
using System.Linq;
using Zepto_API_Backup.Filter;
using Zepto_API_Backup.Services;

namespace Zepto_API_Backup.Controllers
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

        [HttpGet]
        [Route("GetAllProducts")]
        //[AuthAtribute(["Admin", "Customer"])]
        public ActionResult GetAllProducts(string? productName, [FromQuery] CategoryEnum category)
        {
            try
            {
                List<Product> products = _unitOfWork.ProductRepository.ADOGetAllAsync(productName);

                if (category != 0)
                {
                    List<Product> filteredProducts = products
                    .Where(p => p.Category.ToString() == category.ToString())
                    .ToList();
                    products = filteredProducts;
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
        [AuthAtribute(["Admin", "Customer"])]
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
        [AuthAtribute(["Admin"])]
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
