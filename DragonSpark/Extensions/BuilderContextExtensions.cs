using DragonSpark.Runtime.Values;
using Microsoft.Practices.ObjectBuilder2;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class BuilderContextExtensions
	{
		public static bool HasBuildPlan( this IBuilderContext @this ) => @this.Policies.GetNoDefault<IBuildPlanPolicy>( @this.BuildKey, false ) != null;

		public static bool IsBuilding<T>( this IBuilderContext @this ) => IsBuilding( @this, NamedTypeBuildKey.Make<T>() );

		public static bool IsBuilding( this IBuilderContext @this, NamedTypeBuildKey key ) => @this.GetCurrentBuildChain().Contains( key );

		public static NamedTypeBuildKey[] GetCurrentBuildChain( this IBuilderContext @this ) => Ambient.GetCurrentChain<NamedTypeBuildKey>();

		public static T New<T>( this IBuilderContext @this, string name = null )
		{
			using ( new AmbientContextCommand<NamedTypeBuildKey>().ExecuteWith( NamedTypeBuildKey.Make<T>( name ) ) )
			{
				return @this.NewBuildUp<T>( name );
			}
		}
	}
}