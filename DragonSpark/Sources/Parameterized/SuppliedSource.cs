using DragonSpark.Extensions;
using System;

namespace DragonSpark.Sources.Parameterized
{
	public class SuppliedSource<TParameter, TResult> : SourceBase<TResult>
	{
		readonly Func<TParameter, TResult> source;
		readonly Func<TParameter> parameterSource;

		public SuppliedSource( Func<TParameter, TResult> source, TParameter parameter ) : this( source, Factory.For( parameter ) ) {}

		public SuppliedSource( Func<TParameter, TResult> source, Func<TParameter> parameterSource )
		{
			this.source = source;
			this.parameterSource = parameterSource;
		}

		public override TResult Get()
		{
			var parameter = parameterSource();
			var result = parameter.IsAssigned() ? source( parameter ) : default(TResult);
			return result;
		}
	}
}