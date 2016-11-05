using DragonSpark.Application.Setup;
using DragonSpark.Commands;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Windows.Input;

namespace DragonSpark.Application
{
	public class ApplicationCommandSource : SuppliedCommandSource
	{
		readonly IEnumerable<Type> types;

		public ApplicationCommandSource( IEnumerable<Type> types, params ICommandSource[] sources ) : base( sources )
		{
			this.types = types;
		}

		protected override IEnumerable<ICommand> Yield()
		{
			yield return ApplicationPartsFactory.Default.WithParameter( types.ToImmutableArray() ).ToCommand();
			yield return new DisposingCommand( Disposables.Default.Get() );

			foreach ( var command in base.Yield() )
			{
				yield return command;
			}

			yield return new ApplySetup();
		}
	}
}