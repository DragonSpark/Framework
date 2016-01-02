using System.Collections.Generic;
using DragonSpark.Activation.IoC;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Extensions
{
	public static class BuilderContextExtensions
	{
		public static bool HasBuildPlan( this IBuilderContext @this )
		{
			var result = @this.Policies.GetNoDefault<IBuildPlanPolicy>( @this.BuildKey, false ) != null;
			return result;
		}

		public static NamedTypeBuildKey[] GetBuildChain( this IBuilderContext @this )
		{
			var result = new Chain( @this.Strategies ).Item.ToArray();
			return result;
		}

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