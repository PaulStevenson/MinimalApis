using System;
using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Infastructure
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options): base(options){}

		public DbSet<Article> Articles { get; set; }
	}
}

