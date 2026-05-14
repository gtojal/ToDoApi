using System.ComponentModel.DataAnnotations;
using ToDoApi.Enums;

namespace ToDoApi.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        public TaskStatusEnum Status { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
