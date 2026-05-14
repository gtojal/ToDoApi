using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoApi.Data;
using ToDoApi.Repositories;
using ToDoApi.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();



// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// Entity Framework

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection

builder.Services.AddScoped<ITaskRepository, TaskRepository>();


// AutoMapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Build app
var app = builder.Build();



// Configure HTTP pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();