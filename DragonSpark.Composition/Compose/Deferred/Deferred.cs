using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class Deferred : ICommand<IServiceCollection>
{
	readonly ICommand<IServiceCollection>                                     _command;
	readonly ISelect<IServiceCollection, IList<ICommand<IServiceCollection>>?> _registrations;

	public Deferred(ICommand<IServiceCollection> command) : this(command, GetDeferredRegistrations.Default) { }

	public Deferred(ICommand<IServiceCollection> command,
					ISelect<IServiceCollection, IList<ICommand<IServiceCollection>>?> registrations)
	{
		_command       = command;
		_registrations = registrations;
	}

	public void Execute(IServiceCollection parameter)
	{
		var commands = _registrations.Get(parameter);
		if (commands is not null)
		{
			commands.Add(_command);    
		}
		else
		{
			_command.Execute(parameter);
		}
		
	}
}