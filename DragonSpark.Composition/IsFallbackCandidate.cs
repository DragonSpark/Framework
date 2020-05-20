using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Composition
{
	sealed class IsFallbackCandidate : AllCondition<Type>
	{
		public static IsFallbackCandidate Default { get; } = new IsFallbackCandidate();

		IsFallbackCandidate() : base(CanActivate.Default.Get, IsNativeFrameworkType.Default.Then().Inverse()) {}
	}
}