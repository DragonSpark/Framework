using DragonSpark.Sources.Coercion;
using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;

namespace DragonSpark.Activation
{
	public class ConstructCoercer<T> : CoercerBase<T>
	{
		public static ConstructCoercer<T> Default { get; } = new ConstructCoercer<T>();
		ConstructCoercer() : this( ParameterConstructor<T>.From ) {}

		readonly Func<object, T> projector;

		[UsedImplicitly]
		protected ConstructCoercer( Func<object, T> projector )
		{
			this.projector = projector;
		}

		protected override T Coerce( [Optional]object parameter ) => projector( parameter );
	}
}