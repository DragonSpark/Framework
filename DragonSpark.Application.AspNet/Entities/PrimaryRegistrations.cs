using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

sealed class PrimaryRegistrations<T> : Commands<IServiceCollection> where T : DbContext
{
	public static PrimaryRegistrations<T> Default { get; } = new();

	PrimaryRegistrations() : base(Registrations<T>.Default, Transactions.Registrations.Default) {}
}