using System;
using AgendaxApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgendaxApi.Data
{
	public class DataContext :DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
		public DbSet<Project> Projects {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasIndex(u=>u.Email).IsUnique();
			modelBuilder.Entity<User>()
				.HasMany(u => u.ToDos)
				.WithOne(t => t.User)
				.HasForeignKey(t => t.UserId)
				.HasPrincipalKey(u=>u.Id);
			modelBuilder.Entity<User>()
				.HasMany(p => p.Projects)
				.WithOne(t => t.User)
				.HasForeignKey(t => t.UserId)
				.HasPrincipalKey(u=>u.Id);
		}

    }
}

