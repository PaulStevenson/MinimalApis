using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiDemo.Entities
{
	public class ArticleEntity
	{
        public ArticleEntity() { }

         public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishedAt { get; set; }

        public int MyNumber { get; set; }
	}
}
