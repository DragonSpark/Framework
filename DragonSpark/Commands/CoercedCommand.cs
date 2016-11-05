using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Commands
{
	public class CoercedCommand<TFrom, TParameter> : DelegatedCommand<TParameter>, ICommand<TFrom>
	{
		readonly Func<TFrom, TParameter> coercer;

		public CoercedCommand( IParameterizedSource<TFrom, TParameter> coercer, Action<TParameter> source ) : this( coercer.ToDelegate(), source ) {}

		[UsedImplicitly]
		public CoercedCommand( Func<TFrom, TParameter> coercer, Action<TParameter> source ) : base( source )
		{
			this.coercer = coercer;
		}

		public bool IsSatisfiedBy( [Optional]TFrom parameter ) => parameter.IsAssigned();

		public void Execute( [Optional]TFrom parameter )
		{
			if ( IsSatisfiedBy( parameter ) )
			{
				var to = coercer( parameter );
				if ( to != null )
				{
					base.Execute( to );
				}
			}
		}
	}
}