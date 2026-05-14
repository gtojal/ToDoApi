using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Enums;
using ToDoApi.Models;
using ToDoApi.Repositories.Interfaces;

namespace ToDoApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            TaskStatusEnum? status,
            DateTime? dataVencimento)
        {
            IQueryable<TaskItem> query = _context.Tasks;

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (dataVencimento.HasValue)
                query = query.Where(x => x.DataVencimento.Date == dataVencimento.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
