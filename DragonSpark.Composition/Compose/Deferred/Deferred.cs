using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class Deferred : ICommand<IServiceCollection>
{
	readonly ICommand<IServiceCollection>                                     _command;
	readonly ISelect<IServiceCollection, IList<ICommand<IServiceCollection>>> _registrations;

	public Deferred(ICommand<IServiceCollection> command) : this(command, GetDeferredRegistrations.Default) { }

	public Deferred(ICommand<IServiceCollection> command,
	                ISelect<IServiceCollection, IList<ICommand<IServiceCollection>>> registrations)
	{
		_command       = command;
		_registrations = registrations;
	}

	public void Execute(IServiceCollection parameter)
	{
		_registrations.Get(parameter).Add(_command);
	}
}