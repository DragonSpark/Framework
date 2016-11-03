using DragonSpark.Sources.Coercion;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Activation
{
	public class ConstructCoercer<T> : CoercerBase<T>
	{
		public static ConstructCoercer<T> Default { get; } = new ConstructCoercer<T>( ParameterConstructor<T>.From );
		
		readonly Func<object, T> projector;

		protected ConstructCoercer( Func<object, T> projector )
		{
			this.projector = projector;
		}

		protected override T Apply( [Optional]object parameter ) => projector( parameter );
	}
}