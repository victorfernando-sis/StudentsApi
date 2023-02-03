using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Models;

namespace StudentsApi.Context
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{

		}

		public DbSet<Student> Students{ get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasData(
				new Student
				{
					StudentId = 1,
					Name = "Luke Combs",
					Email = "lukecombs@hotmail.com",
					Age = 34
				},
				new Student
				{
					StudentId = 2,
					Name = "Witney Houston",
					Email = "witneyh@hotmail.com",
					Age = 22
				}
				);
		}
	}
}

