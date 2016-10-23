using System;

namespace DragonSpark.Sources.Parameterized
{
	public abstract class AlterationBase<T> : ParameterizedSourceBase<T, T>, IAlteration<T> {}

	public class AlterationAdapter<T> : AlterationBase<T>
	{
		readonly Func<T, T> factory;

		public AlterationAdapter( Func<T, T> factory )
		{
			this.factory = factory;
		}

		public override T Get( T parameter ) => factory( parameter );
	}

	public class AppliedAlteration<T> : DelegatedAlteration<T> where T : class
	{
		public AppliedAlteration( Alter<T> source ) : base( source ) {}

		public override T Get( T parameter ) => base.Get( parameter ) ?? parameter;
	}

	public class DelegatedAlteration<T> : AlterationBase<T>
	{
		readonly Alter<T> source;

		public DelegatedAlteration( Alter<T> source )
		{
			this.source = source;
		}

		public override T Get( T parameter ) => source( parameter );
	}
}