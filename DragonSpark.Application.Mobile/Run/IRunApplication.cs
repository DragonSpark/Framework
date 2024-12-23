using System;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Mobile.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Run;

public interface IRunApplication : IAllocating<InitializeInput, Application>;

public sealed class RunInitializedApplication : IRunApplication
{
	readonly IRunApplication _previous;

	public RunInitializedApplication(IRunApplication previous) => _previous = previous;

	public async Task<Application> Get(InitializeInput parameter)
	{
		var result  = await _previous.Await(parameter);
		var service = result.Host.Services.GetService<IInitializeApplication>();
		if (service is not null)
		{
			await service.Await(result);
		}

		return result;
	}
}

// TODO
public interface IInitializeApplication : IOperation<Application>;

public interface IApplicationErrorHandler : ICommand<Exception>;

sealed class ApplicationErrorHandler : IApplicationErrorHandler
{
	readonly IExceptions _exceptions;
	readonly Type        _owner;

    [ActivatorUtilitiesConstructor]
	public ApplicationErrorHandler(IExceptions exceptions) : this(exceptions, A.Type<ApplicationErrorHandler>()) {}

	public ApplicationErrorHandler(IExceptions exceptions, Type owner)
	{
		_exceptions = exceptions;
		_owner      = owner;
	}

	public void Execute(Exception parameter)
	{
		_exceptions.Allocate(new(_owner, parameter)).GetAwaiter().GetResult();
	}
}

sealed class DefaultInitializeApplication : IInitializeApplication
{
	public static DefaultInitializeApplication Default { get; } = new();

	DefaultInitializeApplication()
        : this(x => CurrentServices.Default.GetRequiredService<IApplicationErrorHandler>().Execute(x)) {}

	readonly Action<Exception> _handler;

	public DefaultInitializeApplication(Action<Exception> handler) => _handler = handler;

	public ValueTask Get(Application parameter)
	{
		return ValueTask.CompletedTask;
	}
}
