using Dapper;
using Microsoft.EntityFrameworkCore;
using Practice_ADO_Dapper.DBContext;
using Practice_ADO_Dapper.Services;
using TodoModels.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Practice_ADO_Dapper.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly SQLDBContext _context;

        public TodoRepository(SQLDBContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var todos = await connection.QueryAsync<Todo>("spTodo_GetAll", commandType: CommandType.StoredProcedure);
                return todos.ToList();
            }
        }

        public async Task<List<Todo>> GetAsync(Guid Id)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = string.Format("SELECT * FROM Todos WHERE TodoId = '{0}';", Id);
                var todoParameter = new DynamicParameters();
                todoParameter.Add("@TodoId", Id);

                var todo = await connection.QueryAsync<Todo>(query, param: todoParameter, commandType: CommandType.Text);

                return todo.ToList();
            }
        }



    }
}
