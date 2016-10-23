using DragonSpark.Sources;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Commands
{
	public interface IConfigurableCommand<T> : ICommand<T>
	{
		IScope<Action<T>> Configuration { get; }
	}

	public class ConfigurableCommand<T> : DelegatedCommand<T>, IConfigurableCommand<T>
	{
		public ConfigurableCommand( Action<T> command ) : this( new Scope<Action<T>>( command.Self ) ) {}

		[UsedImplicitly]
		public ConfigurableCommand( IScope<Action<T>> configuration ) : base( configuration.Execute )
		{
			Configuration = configuration;
		}

		public IScope<Action<T>> Configuration { get; }
	}
}