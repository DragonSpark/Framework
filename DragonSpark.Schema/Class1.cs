using DragonSpark.Application.AspNet.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

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

		[MaxLength(256)]
		public string Message { get; set; } = default!;
	}
}