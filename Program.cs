using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));

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

app.MapGet("/", () => "Hello World!");

app.MapGet("/articles", async (
    ApiContext context) => Results.Ok(
        await context.Articles.ToListAsync())
    );
app.MapGet("/articles/{id}", async(
    int id,
    ApiContext context) =>
{
    var article = await context.Articles.FindAsync(id);

    return article != null ? Results.Ok(article) : Results.NotFound();
});

app.MapPost("/articles", async (ArticleRequest article, ApiContext context) =>
{
    var newArticle = new Article
    {
        Title = article.Title ?? string.Empty,
        Content = article.Content ?? string.Empty,
        PublishedAt = article.PublishedAt ?? DateTime.Now
    };

    var createdArticle = context.Articles.Add(newArticle);

    await context.SaveChangesAsync();

    return Results.Created($"articles/{createdArticle.Entity.Id}", createdArticle.Entity);
});

app.MapDelete("articles/{id}", async (int id, ApiContext context) =>
{
    var article = await context.Articles.FindAsync(id);

    if (article == null)
    {
        return Results.NotFound($"Cannot by article {id}");
    }

    context.Articles.Remove(article);

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/articles/{id}", async (int id, ArticleRequest article, ApiContext context) =>
{
    var articleToUpdate = await context.Articles.FindAsync(id);

    if (articleToUpdate == null)
        return Results.NotFound();

    if (article.Title != null)
        articleToUpdate.Title = article.Title;

    if (article.Content != null)
        articleToUpdate.Content = article.Content;

    if (article.PublishedAt != null)
        articleToUpdate.PublishedAt = article.PublishedAt;

    await context.SaveChangesAsync();

    return Results.Ok(articleToUpdate);
});

app.Run();

