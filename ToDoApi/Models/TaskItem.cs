using ToDoApi.Enums;

namespace ToDoApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public TaskStatusEnum Status { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
