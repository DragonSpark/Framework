using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.Compose.Communication
{
	sealed class AddRefit<T> : ICommand<IServiceCollection> where T : class, IResult<IHttpContentSerializer>
	{
		public static AddRefit<T> Default { get; } = new AddRefit<T>();

		AddRefit() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IHttpContentSerializer>()
			         .Use<T>()
			         .Scoped()
			         .Then.Start<RefitSettings>()
			         .Use<RefitConfiguration>()
			         .Scoped();
		}
	}
}