﻿using AutoMapper.Internal;
using DragonSpark.Diagnostics;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	[ContentProperty( nameof(Commands) )]
	public abstract class Setup : ISetup
	{
	    public Collection<object> Items { get; } = new Collection<object>();

		public virtual CommandCollection Commands { get; } = new CommandCollection();
		
		public virtual void Run( object arguments = null )
		{
			using ( var context = CreateContext( arguments ) )
			{
				Commands
					.Where( command => command.CanExecute( context ) )
					.Each( command => command.Execute( context ) );
			}
		}

		protected virtual SetupContext CreateContext( object arguments )
		{
			var result = new SetupContext( arguments );
			result.Register<ISetup>( this );
			return result;
		}
	}

	public abstract class Setup<TLogger, TModuleCatalog> : Setup
		where TLogger : ILogger, new()
		where TModuleCatalog : IModuleCatalog, new()
	{
		public override CommandCollection Commands { get; } = new CommandCollection( new ICommand[]
			{
				new SetupLoggingCommand<TLogger>(),
				new SetupModuleCatalogCommand<TModuleCatalog>(),
				new SetupUnityCommand(),
				new RegisterFrameworkExceptionTypesCommand()
			} );
	}
}
