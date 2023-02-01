using System;
namespace MinimalApiDemo.Services
{
	public interface IArticleService
	{
        Task<IResult> GetAll();
	}
}

