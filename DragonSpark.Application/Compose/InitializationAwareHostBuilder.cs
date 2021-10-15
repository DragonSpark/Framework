using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Compose;

sealed class InitializationAwareHostBuilder<T> : InitializationAwareHostBuilder
{
	public InitializationAwareHostBuilder(Func<IHostBuilder, IHostBuilder> previous)
		: base(previous, EmitBuilding<T>.Default, EmitBuilt<T>.Default) {}
}

class InitializationAwareHostBuilder : IAlteration<IHostBuilder>
{
	readonly Func<IHostBuilder, IHostBuilder> _previous;
	readonly ICommand                         _initializing, _initialized;

	public InitializationAwareHostBuilder(Func<IHostBuilder, IHostBuilder> previous, ICommand initializing,
	                                      ICommand initialized)
	{
		_previous     = previous;
		_initializing = initializing;
		_initialized  = initialized;
	}

	public IHostBuilder Get(IHostBuilder parameter)
	{
		_initializing.Execute();
		return new InitializedAwareHostBuilder(_previous(parameter), _initialized);
	}
}