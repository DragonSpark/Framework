using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities
{
	sealed class Registrations<T> : ICommand<IServiceCollection> where T : DbContext
	{
		public static Registrations<T> Default { get; } = new();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IContexts<T>>()
			         .Forward<Contexts<T>>()
			         .Singleton()
					 //
			         .Then.AddScoped(typeof(ISave<>), typeof(Save<>));
		}
	}
}