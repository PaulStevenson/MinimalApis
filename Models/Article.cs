using System;
namespace MinimalApiDemo.Models
{
	public class Article
	{
		public Article()
		{
		}

		public int Id { get; set; }

		public string? title { get; set; }

		public string? Content { get; set; }

		public DateTime? PublishedAt { get; set; }
	}

    public record ArticleRequest(string? Title, string? Content, DateTime? PublishedAt);
}

