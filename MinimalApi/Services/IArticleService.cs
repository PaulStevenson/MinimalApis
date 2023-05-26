using System;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public interface IArticleService
	{
        Task<IList<Article>> GetAll();

		Task<Article> GetById(int id);

        Task<Article> Post(ArticleRequest article);

        Task<Article> PostWithValidation(Article newArticle);

        Task<Article> Delete(int id);

		Task<Article> Put(int id, ArticleRequest article);
    }
}

