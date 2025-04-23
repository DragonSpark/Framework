using System;
using DragonSpark.Application.Mobile.Uno.Presentation;
using DragonSpark.Application.Mobile.Uno.Run;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Command = Uno.Extensions.Reactive.Command;

namespace DragonSpark.Application.Mobile.Uno;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

	DefaultRegistrations()
		: this(x => CurrentServices.Default.GetRequiredService<IApplicationErrorHandler>().Execute(x)) {}

	readonly Action<Exception> _error;

	public DefaultRegistrations(Action<Exception> error) => _error = error;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IApplicationErrorHandler>()
				 .Forward<ApplicationErrorHandler>()
				 .Singleton()
				 //
				 .Then.Start<IInitializeApplication>()
				 .Forward<DefaultInitializeApplication>()
				 .Singleton()
			;
		Command.DefaultErrorHandler = _error;
	}
}
