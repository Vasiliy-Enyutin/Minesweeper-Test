using FluentValidation;
using FluentValidation.AspNetCore;
using Minesweeper.API.Middleware;
using Minesweeper.Application.Services;
using Minesweeper.Application.Validators;
using Minesweeper.Domain.Interfaces;
using Minesweeper.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// singleton т.к. необходимо хранить данные об игре между запросами
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<NewGameRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TurnRequestValidator>();

builder.Services.AddCors(options => 
    options.AddPolicy("AllowAll", p => 
        p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();