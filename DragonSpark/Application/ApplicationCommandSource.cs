using DragonSpark.Application.Setup;
using DragonSpark.Commands;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonSpark.Application
{
	public class ApplicationCommandSource : SuppliedCommandSource
	{
		readonly IEnumerable<Type> types;
		public ApplicationCommandSource( IEnumerable<Type> types ) : this( types, Items<ICommandSource>.Default ) {}
		public ApplicationCommandSource( IEnumerable<Type> types, params ICommandSource[] sources ) : base( sources )
		{
			this.types = types;
		}

		protected override IEnumerable<ICommand> Yield()
		{
			yield return ApplicationParts.Default.Configured( SystemPartsFactory.Default.Get( types ) );
			yield return new DisposeDisposableCommand( Disposables.Default.Get() );

			foreach ( var command in base.Yield() )
			{
				yield return command;
			}

			yield return new ApplySetup();
		}
	}

	
}