using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApi.Controllers;
using ToDoApi.DTOs;
using ToDoApi.Enums;
using ToDoApi.Models;
using ToDoApi.Repositories.Interfaces;

namespace ToDoApi.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly TasksController _controller;

        public TaskControllerTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _mapperMock = new Mock<IMapper>();

            _controller = new TasksController(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenTaskIsValid()
        {
            // Arrange

            var createDto = new CreateTaskDto
            {
                Titulo = "Criar projeto desafio técnico ToDo Api",
                Descricao = "Desafio Técnico",
                Status = TaskStatusEnum.EmAberto,
                DataVencimento = DateTime.Now.AddDays(1)
            };

            var task = new TaskItem
            {
                Id = 1,
                Titulo = createDto.Titulo,
                Descricao = createDto.Descricao,
                Status = createDto.Status,
                DataVencimento = createDto.DataVencimento
            };

            var responseDto = new TaskResponseDto
            {
                Id = 1,
                Titulo = task.Titulo,
                Descricao = task.Descricao,
                Status = task.Status.ToString(),
                DataVencimento = task.DataVencimento
            };

            _mapperMock
                .Setup(x => x.Map<TaskItem>(createDto))
                .Returns(task);

            _mapperMock
                .Setup(x => x.Map<TaskResponseDto>(task))
                .Returns(responseDto);

            // Act

            var result = await _controller.Create(createDto);

            // Assert

            result.Result.Should().BeOfType<CreatedAtActionResult>();

            var createdResult = result.Result as CreatedAtActionResult;

            createdResult!.StatusCode.Should().Be(201);

            _repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<TaskItem>()),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange

            var task = new TaskItem
            {
                Id = 1,
                Titulo = "Criar projeto desafio técnico ToDo Api",
                Descricao = "Desafio Técnico",
                Status = TaskStatusEnum.Pendente,
                DataVencimento = DateTime.Now.AddDays(1)
            };

            var responseDto = new TaskResponseDto
            {
                Id = task.Id,
                Titulo = task.Titulo,
                Descricao = task.Descricao,
                Status = task.Status.ToString(),
                DataVencimento = task.DataVencimento
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(task);

            _mapperMock
                .Setup(x => x.Map<TaskResponseDto>(task))
                .Returns(responseDto);

            // Act

            var result = await _controller.GetById(1);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var returnedTask = Assert.IsType<TaskResponseDto>(okResult.Value);

            Assert.Equal(task.Id, returnedTask.Id);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenTaskUpdated()
        {
            // Arrange

            var task = new TaskItem
            {
                Id = 1,
                Titulo = "Criar projeto desafio técnico ToDo Api",
                Descricao = "Desafio Técnico",
                Status = TaskStatusEnum.Pendente,
                DataVencimento = DateTime.Now.AddDays(1)
            };

            var updateDto = new UpdateTaskDto
            {
                Titulo = "Criar projeto desafio técnico ToDo Api - Mirante",
                Descricao = "Desafio Técnico - Mirante",
                Status = TaskStatusEnum.Concluido,
                DataVencimento = DateTime.Now.AddDays(2)
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(task);

            // Act

            var result = await _controller.Update(1, updateDto);

            // Assert

            Assert.IsType<NoContentResult>(result);

            _repositoryMock.Verify(
                x => x.UpdateAsync(task),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenTaskDeleted()
        {
            // Arrange

            var task = new TaskItem
            {
                Id = 1,
                Titulo = "Criar projeto desafio técnico ToDo Api",
                Descricao = "Desafio Técnico",
                Status = TaskStatusEnum.Pendente,
                DataVencimento = DateTime.Now.AddDays(1)
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(task);

            // Act

            var result = await _controller.Delete(1);

            // Assert

            Assert.IsType<NoContentResult>(result);

            _repositoryMock.Verify(
                x => x.DeleteAsync(task),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnFilteredTasks()
        {
            // Arrange

            var tasks = new List<TaskItem>
            {
                new()
                {
                    Id = 1,
                    Titulo = "Task 1",
                    Descricao = "Desafio Técnico",
                    Status = TaskStatusEnum.Pendente,
                    DataVencimento = DateTime.Now.AddDays(1)
                },
                new()
                {
                    Id = 2,
                    Titulo = "Task 2",
                    Descricao = "Desafio Técnico",
                    Status = TaskStatusEnum.Concluido,
                    DataVencimento = DateTime.Now.AddDays(1)
                },
                new()
                {
                    Id = 3,
                    Titulo = "Task 3",
                    Descricao = "Desafio Técnico",
                    Status = TaskStatusEnum.EmAberto,
                    DataVencimento = DateTime.Now.AddDays(1)
                }
            };

            var responseDtos = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Status = t.Status.ToString()
            }).ToList();

            _repositoryMock
                .Setup(x => x.GetAllAsync(
                    TaskStatusEnum.Pendente,
                    null))
                .ReturnsAsync(tasks.Where(x => x.Status == TaskStatusEnum.Pendente));

            _mapperMock
                .Setup(x => x.Map<IEnumerable<TaskResponseDto>>(It.IsAny<IEnumerable<TaskItem>>()))
                .Returns(responseDtos);

            // Act

            var result = await _controller.GetAll(TaskStatusEnum.Pendente, null);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.NotNull(okResult.Value);
        }

    }
}