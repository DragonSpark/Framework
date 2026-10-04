using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

sealed class Registrations<T> : Commands<IServiceCollection> where T : DbContext
{
	public static Registrations<T> Default { get; } = new();

	Registrations() : base(PrimaryConfiguration<T>.Default, GeneralConfiguration<T>.Default) {}
}