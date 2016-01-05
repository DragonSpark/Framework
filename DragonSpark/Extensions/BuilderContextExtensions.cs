using System;
using System.Collections.Generic;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Extensions
{
	public static class BuilderContextExtensions
	{
		/*public static Exception Try( this IBuilderContext @this, Action action ) => @this.NewBuildUp<TryContext>().Try( action );*/

		public static bool HasBuildPlan( this IBuilderContext @this ) => @this.Policies.GetNoDefault<IBuildPlanPolicy>( @this.BuildKey, false ) != null;

		public static NamedTypeBuildKey[] GetBuildChain( this IBuilderContext @this ) => new Chain( @this.Strategies ).Item.ToArray();

		/*public static bool IsRegistered( this IBuilderContext @this, NamedTypeBuildKey key )
		{
			var policy = @this.Policies.GetNoDefault<IBuildKeyMappingPolicy>( key, false );
			var result = policy != null;
			return result;
		}

		public static bool IsRegistered<T>( this IBuilderContext @this )
		{
			var result = @this.IsRegistered( new NamedTypeBuildKey( typeof(T) ) );
			return result;
		}*/
	}
}