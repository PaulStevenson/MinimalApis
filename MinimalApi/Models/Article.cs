using System;
using System.ComponentModel.DataAnnotations;

namespace MinimalApiDemo.Models
{
	public class Article
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime? PublishedAt { get; set; }

		public int? MyNumber { get; set; }
	}

    public record ArticleRequest(string Title, string Content, DateTime PublishedAt, int MyNumber);


}

