using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Entities;
using MinimalApiDemo.Infastructure;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public class ArticleService : IArticleService
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ArticleService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<Article>> GetAll()
        {
            var entities = await _context.Articles.ToListAsync();
            var results = _mapper.Map<IList<Article>>(entities);

            return results;
        }

        public async Task<Article> GetById(int id)
        {
            var articleEntity = await _context.Articles.FindAsync(id);
            var result = _mapper.Map<Article>(articleEntity);

            return result;
        }

        public async Task<IResult> Post(ArticleRequest article)
        {
            var title = String.IsNullOrEmpty(article.Title) ? "Default Title" : article.Title;

            var newArticle = new Article
            {
                Title = title,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
                MyNumber = article.MyNumber
            };

            var entity = _mapper.Map<ArticleEntity>(newArticle);

            var createdArticle = _context.Articles.Add(entity);

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

