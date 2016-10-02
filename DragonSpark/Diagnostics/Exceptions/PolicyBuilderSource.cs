using DragonSpark.Sources;
using Polly;
using System;

namespace DragonSpark.Diagnostics.Exceptions
{
	public sealed class PolicyBuilderSource<T> : DelegatedSource<PolicyBuilder> where T : Exception
	{
		public static PolicyBuilderSource<T> Default { get; } = new PolicyBuilderSource<T>();
		PolicyBuilderSource() : base( Policy.Handle<T> ) {}
	}
}