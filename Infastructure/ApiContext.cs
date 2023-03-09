using System;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Entities;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Infastructure
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options): base(options){}

		public DbSet<ArticleEntity> Articles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ArticleEntity>()
				.Property(a => a.Title)
				.IsRequired();

			modelBuilder.Entity<ArticleEntity>()
				.Property(a => a.Content)
				.IsRequired();
        }
	}
}