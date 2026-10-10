using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

sealed class GeneralConfiguration<T> : ICommand<IServiceCollection> where T : DbContext
{
	public static GeneralConfiguration<T> Default { get; } = new();

	GeneralConfiguration() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<INewContext<T>>()
		         .Forward<NewContext<T>>()
		         .Singleton()
		         //
		         .Then.Start<INewContext>()
		         .Forward<NewStandardContext<T>>()
		         .Singleton();
	}
}