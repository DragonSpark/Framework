using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Markup;
using Defaults = DragonSpark.Sources.Parameterized.Defaults;

namespace DragonSpark.Commands
{
	public class CompositeCommand : CompositeCommand<object>, IRunCommand
	{
		public CompositeCommand() : this( Items<ICommand>.Default ) {}

		public CompositeCommand( params ICommand[] commands ) : base( commands ) {}

		public void Execute() => Execute( Defaults.Parameter );
	}

	[ContentProperty( nameof(Commands) )]
	public class CompositeCommand<T> : DisposingCommand<T>
	{
		public CompositeCommand( params ICommand[] commands )
		{
			Commands = new CommandCollection( commands );
		}

		public CommandCollection Commands { get; }

		public override void Execute( T parameter )
		{
			foreach ( var command in Commands.ToArray() )
			{
				command.Execute( parameter );
			}
		}

		protected override void OnDispose() => Commands.Purge().OfType<IDisposable>().Reverse().Each( disposable => disposable.Dispose() );
	}
}