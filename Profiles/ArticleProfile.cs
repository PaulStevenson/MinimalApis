using System;
using AutoMapper;
using MinimalApiDemo.Entities;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Profiles
{
	public class ArticleProfile : Profile
    {
		public ArticleProfile()
		{
			CreateMap<Article, ArticleEntity>();
            CreateMap<ArticleEntity, Article>();
        }
	}
}

