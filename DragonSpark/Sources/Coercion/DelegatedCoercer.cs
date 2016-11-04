using System;

namespace DragonSpark.Sources.Coercion
{
	public class DelegatedCoercer<TFrom, TTo> : CoercerBase<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> factory;

		public DelegatedCoercer( Func<TFrom, TTo> factory )
		{
			this.factory = factory;
		}

		protected override TTo Coerce( TFrom parameter ) => factory( parameter );
	}
}