using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;
using MinimalApiDemo.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));

// IoC Reg
builder.Services.AddScoped<IArticleService, ArticleService>();

var app = builder.Build();

{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.UseHttpsRedirection();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// Controllers
app.MapGet("/articles", async (
    IArticleService articleService) => 
await articleService.GetAll()
    );

app.MapGet("/articles/{id}", async (
    int id,
    IArticleService articleService) =>
await articleService.GetById(id));

app.MapPost("/articles", async (
    ArticleRequest article,
    IArticleService articleService) =>
await articleService.Post(article));

app.MapDelete("articles/{id}", async (
    int id,
    IArticleService articleService) =>
await articleService.Delete(id));

app.MapPut("/articles/{id}", async (int id, ArticleRequest article, IArticleService articleService) =>
await articleService.Put(id, article));

// Starting point
app.Run();

