﻿using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using Polly;
using System;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Diagnostics.Exceptions
{
	public class SuppliedRetryPolicySource<T> : SuppliedSource<int, Policy> where T : Exception
	{
		const int Retries = 5;

		public static ISource<Policy> Default { get; } = new Scope<Policy>( new SuppliedRetryPolicySource<T>().GlobalCache() );
		SuppliedRetryPolicySource() : this( Retries ) {}

		[UsedImplicitly]
		public SuppliedRetryPolicySource( int parameter ) : this( RetryPolicySource<T>.Default, parameter ) {}

		[UsedImplicitly]
		public SuppliedRetryPolicySource( IParameterizedSource<int, Policy> source, int parameter = Retries ) : base( source.Get, parameter ) {}
	}
}
