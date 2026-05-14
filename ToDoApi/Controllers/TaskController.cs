using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoApi.DTOs;
using ToDoApi.Enums;
using ToDoApi.Models;
using ToDoApi.Repositories.Interfaces;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Endpoints responsáveis pelo gerenciamento de tarefas")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TasksController(
            ITaskRepository taskRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca tarefa por ID
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                return NotFound($"Tarefa com ID {id} não localizada.");

            var response = _mapper.Map<TaskResponseDto>(task);

            return Ok(response);
        }

        /// <summary>
        /// Lista todas as tarefas
        /// </summary>
        /// <returns>Lista de tarefas cadastradas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
            [FromQuery] TaskStatusEnum? status,
            [FromQuery] DateTime? dataVencimento)
        {
            var tasks = await _taskRepository.GetAllAsync(status, dataVencimento);

            var response = _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);

            return Ok(response);
        }

        /// <summary>
        /// Cria uma nova tarefa
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskResponseDto>> Create(
            [FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = _mapper.Map<TaskItem>(dto);

            await _taskRepository.AddAsync(task);

            await _taskRepository.SaveChangesAsync();

            var response = _mapper.Map<TaskResponseDto>(task);

            return CreatedAtAction(
                nameof(GetById),
                new { id = task.Id },
                response);
        }


        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(
            int id,
            [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTask = await _taskRepository.GetByIdAsync(id);

            if (existingTask == null)
                return NotFound($"Tarefa com ID {id} não localizada.");

            _mapper.Map(dto, existingTask);

            await _taskRepository.UpdateAsync(existingTask);

            await _taskRepository.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Remove uma tarefa
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                return NotFound($"Tarefa com ID {id} não localizada.");

            await _taskRepository.DeleteAsync(task);

            await _taskRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}