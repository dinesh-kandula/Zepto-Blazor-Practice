//using Microsoft.AspNetCore.Mvc;
//using Practice_ADO_Dapper.Services;
//using TodoModels.Models;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Practice_ADO_Dapper.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TodoController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public TodoController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var todos = await _unitOfWork.TodoRepository.GetAllAsync();
//            return Ok(todos);
//        }

//        // GET: api/<TodoController>
//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get([FromRoute] Guid Id)
//        {
//            var todo = await _unitOfWork.TodoRepository.GetAsync(Id);
//            return Ok(todo);
//        }

//        [HttpGet("/users")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = await _unitOfWork.UserRepository.GetAllAsync();
//            return Ok(users);
//        }

//        //// GET api/<TodoController>/5
//        //[HttpGet("{id}")]
//        //public string Get(int id)
//        //{
//        //    return "value";
//        //}

//        //// POST api/<TodoController>
//        //[HttpPost]
//        //public void Post([FromBody] string value)
//        //{
//        //}

//        //// PUT api/<TodoController>/5
//        //[HttpPut("{id}")]
//        //public void Put(int id, [FromBody] string value)
//        //{
//        //}

//        //// DELETE api/<TodoController>/5
//        //[HttpDelete("{id}")]
//        //public void Delete(int id)
//        //{
//        //}
//    }
//}
