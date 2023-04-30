using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly IConfiguration _configuration;
        public TodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> Get()
        {
            string query = "select * from TodoApi";
            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            var data = await connection.QueryAsync<Todo>(query);
            return data.ToList();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> Get(string id)
        {
            string query = $"select * from TodoAPI where Id = '{id}'";
            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            return await connection.QuerySingleOrDefaultAsync<Todo>(query);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Post([FromBody] Todo todo)
        {
            string query = $"insert into TodoApi values ('{todo.Id}' ,'{todo.Name}' , '{todo.Description}' , '{todo.CreatedAt}' , '{todo.UpdatedAt}' )";
            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await connection.ExecuteAsync(query);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Put(string id, [FromBody] Todo todo)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            if (id != todo.Id)
                return BadRequest();

            todo.UpdatedAt = DateTime.Now.ToString();
            string query = $"update TodoApi set Name = '{todo.Name}' , Description = '{todo.Description}' ,UpdatedAt = '{todo.UpdatedAt}' where Id = '{todo.Id}'";
            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await connection.ExecuteAsync(query);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Todo>> Delete(string id)
        {
            string query = $"delete from TodoAPI where Id = '{id}'";
            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await connection.ExecuteAsync(query);
            return NoContent();
        }
    }
}
