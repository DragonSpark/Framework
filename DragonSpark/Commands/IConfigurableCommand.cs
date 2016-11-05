using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Commands
{
	public interface IConfigurableCommand<T> : ICommand<T>
	{
		IScope<Action<T>> Configuration { get; }
	}

	public class ScopedCommand<T> : DelegatedCommand<T>, IConfigurableCommand<T>
	{
		public ScopedCommand( Action<T> command ) : this( new Scope<Action<T>>( command.Self ) ) {}

		[UsedImplicitly]
		public ScopedCommand( IScope<Action<T>> configuration ) : base( configuration.Execute )
		{
			Configuration = configuration;
		}

		public IScope<Action<T>> Configuration { get; }
	}
}