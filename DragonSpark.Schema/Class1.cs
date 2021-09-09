using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Schema
{
	public class Class1 {}

	public sealed class ApplicationState : Entities
	{
		public ApplicationState(DbContextOptions<ApplicationState> options) : base(options) {}

		public DbSet<Subject> Subjects { get; set; } = default!;
	}

	public sealed class Subject
	{
		public Guid Id { get; set; }

		public DateTimeOffset Created { get; set; }

		public string Message { get; set; } = default!;
	}
}