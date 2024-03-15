using Microsoft.AspNetCore.Mvc;
using ModelsClassLibrary.Repository;

namespace ZeptoApi.Controllers
{
    [Route("zepto/[controller]")]
    [ApiController]
    public class PizzaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PizzaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllPizzas")]
        public async Task<ActionResult> GetAllPizzas()
        {
            try
            {
                return Ok(await _unitOfWork.PizzaRepository.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("GetPizzaById/{Id}")]
        public async Task<ActionResult> GetPizzaById(int Id)
        {
            try
            {
                var pizzaDetails = await _unitOfWork.PizzaRepository.GetAsync(Id);
                if(pizzaDetails == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                    $"No Pizza Found with given Id: {Id}");
                }
                return Ok(pizzaDetails);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
