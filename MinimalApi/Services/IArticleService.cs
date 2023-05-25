using System;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public interface IArticleService
	{
        Task<IList<Article>> GetAll();

		Task<Article> GetById(int id);

        Task<Article> Post(ArticleRequest article);

		Task<IResult> PostWithValidation(Article newArticle);

        Task<IResult> Delete(int id);

		Task<IResult> Put(int id, ArticleRequest article);
    }
}

