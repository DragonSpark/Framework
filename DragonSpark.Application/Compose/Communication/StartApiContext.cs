using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace DragonSpark.Application.Compose.Communication
{
	public sealed class StartApiContext<T> where T : class
	{
		readonly IServiceCollection _subject;

		public StartApiContext(IServiceCollection subject) => _subject = subject;

		public RegisterApiContext<T> UsingConfiguration() 
			=> Using(x => x.GetRequiredService<RefitConfiguration>().Get());

		public RegisterApiContext<T> Using(Func<IServiceProvider, RefitSettings?> settings) => new(_subject, settings);
	}

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