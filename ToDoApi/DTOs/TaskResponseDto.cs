namespace ToDoApi.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public string Status { get; set; }

        public DateTime DataVencimento { get; set; }
    }
}
