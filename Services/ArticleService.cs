using System;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Infastructure;

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
    }
}

