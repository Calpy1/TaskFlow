using Microsoft.AspNetCore.Mvc;
using TaskFlowTaskServer.Data;
using TaskFlowTaskServer.Models;
using TaskFlowTaskServer.Controllers;

namespace TaskFlowTaskServer.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateTaskAsync([FromBody] TaskModel taskModel)
        {
            bool valid = await taskModel.AddTaskAsync();

            if (valid)
            {
                return Ok("Successfully created");
            }
            return BadRequest("Failed to create");
        }

        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasksController()
        {
            TaskModel taskModel = new TaskModel();
            var tasks = await taskModel.GetTasksAsync();
            return Ok(tasks);
        }
    }
}
