using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Commands
{
	public class CoercedCommand<TFrom, TParameter> : DelegatedCommand<TParameter>, ICommand<TFrom>
	{
		readonly Func<TFrom, TParameter> coercer;

		public CoercedCommand( IParameterizedSource<TFrom, TParameter> coercer, Action<TParameter> source ) : this( coercer.ToDelegate(), source ) {}

		public CoercedCommand( Func<TFrom, TParameter> coercer, Action<TParameter> source ) : base( source )
		{
			this.coercer = coercer;
		}

		public bool IsSatisfiedBy( TFrom parameter ) => parameter.IsAssigned();

		public void Execute( TFrom parameter )
		{
			var to = coercer( parameter );
			if ( to != null )
			{
				base.Execute( to );
			}
		}
	}
}