﻿using System;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public interface IArticleService
	{
        Task<IList<Article>> GetAll();

		Task<Article> GetById(int id);

		Task<IResult> Post(ArticleRequest article);

		Task<IResult> Delete(int id);

		Task<IResult> Put(int id, ArticleRequest article);
	}
}

