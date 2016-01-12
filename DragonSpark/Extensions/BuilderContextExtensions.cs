using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Extensions
{
	public static class BuilderContextExtensions
	{
		public static bool HasBuildPlan( this IBuilderContext @this ) => @this.Policies.GetNoDefault<IBuildPlanPolicy>( @this.BuildKey, false ) != null;

		public static T New<T>( this IBuilderContext @this, string name = null )
		{
			using ( new AmbientContextCommand<NamedTypeBuildKey>().ExecuteWith( NamedTypeBuildKey.Make<T>( name ) ) )
			{
				return @this.NewBuildUp<T>( name );
			}
		}
	}
}