using System;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public class ArticleService : IArticleService
    {
        private readonly ApiContext _context;

        public ArticleService(ApiContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetAll()
        {
            var articles = await _context.Articles.ToListAsync();

            return Results.Ok(articles);
        }

        public async Task<IResult> GetById(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            return article != null ? Results.Ok(article) : Results.NotFound();
        }

        public async Task<IResult> Post(ArticleRequest article)
        {
            var title = String.IsNullOrEmpty(article.Title) ? "Default Title" : article.Title;
            var content = String.IsNullOrEmpty(article.Content) ? "Default Content" : article.Content;

            var newArticle = new Article
            {
                Title = title,
                Content = content,
                PublishedAt = article.PublishedAt ?? DateTime.Now
            };

            var createdArticle = _context.Articles.Add(newArticle);

            await _context.SaveChangesAsync();

            return Results.Created($"articles/{createdArticle.Entity.Id}", createdArticle.Entity);
        }

        public async Task<IResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return Results.NotFound($"Cannot by article {id}");
            }

            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();

            return Results.NoContent();
        }

        public async  Task<IResult> Put(int id, ArticleRequest article)
        {
            var articleToUpdate = await _context.Articles.FindAsync(id);

            if (articleToUpdate == null)
                return Results.NotFound();

            if (article.Title != null)
                articleToUpdate.Title = article.Title;

            if (article.Content != null)
                articleToUpdate.Content = article.Content;

            if (article.PublishedAt != null)
                articleToUpdate.PublishedAt = article.PublishedAt;

            await _context.SaveChangesAsync();

            return Results.Ok(articleToUpdate);
        }
    }
}

