using ToDoApi.Enums;
using ToDoApi.Models;

namespace ToDoApi.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(
            TaskStatusEnum? status,
            DateTime? dataVencimento);

        Task<TaskItem?> GetByIdAsync(int id);

        Task AddAsync(TaskItem task);

        Task UpdateAsync(TaskItem task);

        Task DeleteAsync(TaskItem task);

        Task SaveChangesAsync();
    }
}
