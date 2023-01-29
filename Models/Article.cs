using System;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiDemo.Models
{
	public class Article
	{
		public Article()
		{
		}

		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public DateTime PublishedAt { get; set; }
	}

    public record ArticleRequest(string? Title, string? Content, DateTime? PublishedAt);
}

