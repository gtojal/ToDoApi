using ToDoApi.DTOs;
using ToDoApi.Models;
using AutoMapper;

namespace ToDoApi.Mappings
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<CreateTaskDto, TaskItem>();

            CreateMap<UpdateTaskDto, TaskItem>();

            CreateMap<TaskItem, TaskResponseDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}