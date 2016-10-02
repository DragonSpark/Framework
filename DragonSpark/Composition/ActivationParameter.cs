using DragonSpark.Activation;
using System;

namespace DragonSpark.Composition
{
	public struct ActivationParameter
	{
		public ActivationParameter( IActivator provider, Type sourceType )
		{
			Services = provider;
			SourceType = sourceType;
		}

		public IActivator Services { get; }
		public Type SourceType { get; }
	}
}