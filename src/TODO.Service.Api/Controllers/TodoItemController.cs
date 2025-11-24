using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Service.Application.Models.TodoItems;
using Todo.Service.Persistence;

namespace backend_dotnet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {

        public readonly ITodoContext _context;

        public TodoItemController(ITodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<TodoItemResponse>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ProblemDetails))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<List<TodoItemResponse>>> ListAll()
        {

            //// Implementation to retrieve and return the list of TodoItemResponse
            //var todoItems = new List<TodoItemResponse>
            //{
            //    new TodoItemResponse
            //    {
            //        Title = "Sample Task",
            //        Description = "This is a sample task description.",
            //        IsDone = false,
            //        DueDate = DateTime.UtcNow.AddDays(7),
            //        Active = true
            //    }
            //};
            //return Ok(todoItems);

            try
            {
                var todoItems = await _context.TodoItems.
                    Where(t => t.Active).
                    ProjectToType<TodoItemResponse>().
                    ToListAsync();


                return Ok(todoItems);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = ex.Message,
                    Status = (int)HttpStatusCode.InternalServerError
                });
            }
        }
    }
}
