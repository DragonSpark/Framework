using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Diagnostics;

namespace DragonSpark.Activation.IoC
{
	public class BuildPlanCreatorPolicy : IBuildPlanCreatorPolicy
	{
		readonly IList<IBuildPlanPolicy> policies;
		readonly IBuildPlanCreatorPolicy[] creators;

		public BuildPlanCreatorPolicy( IList<IBuildPlanPolicy> policies,  params IBuildPlanCreatorPolicy[] creators )
		{
			this.policies = policies;
			this.creators = creators;
		}

		public IBuildPlanPolicy CreatePlan( IBuilderContext context, NamedTypeBuildKey buildKey ) => 
			new CompositeBuildPlanPolicy( creators.Select( policy => policy.CreatePlan( context, buildKey ) ).Concat( policies ).ToArray() );
	}

	public class CompositeBuildPlanPolicy : IBuildPlanPolicy
	{
		readonly IBuildPlanPolicy[] policies;

		public CompositeBuildPlanPolicy( params IBuildPlanPolicy[] policies )
		{
			this.policies = policies;
		}

		public void BuildUp( IBuilderContext context )
		{
			Exception first = null;
			foreach ( var exception in policies.Select( policy => ExceptionSupport.Try( () => policy.BuildUp( context ) ) ) )
			{
				if ( exception == null )
				{
					return;
				}
				first = exception;
			}
			throw first;
		}
	}

	class SingletonBuildPlanPolicy : IBuildPlanPolicy
	{
		readonly ISingletonLocator locator;

		public SingletonBuildPlanPolicy() : this( SingletonLocator.Instance )
		{ }

		public SingletonBuildPlanPolicy( ISingletonLocator locator )
		{
			this.locator = locator;
		}


		public void BuildUp( IBuilderContext context )
		{
			var singleton = locator.Locate( context.BuildKey.Type );
			if ( singleton != null )
			{
				context.Existing = singleton;
			}
		}
	}

	public class EnumerableResolutionStrategy : BuilderStrategy
	{
		delegate object Resolver( IBuilderContext context );

		readonly static MethodInfo GenericResolveArrayMethod = typeof(EnumerableResolutionStrategy).GetTypeInfo().DeclaredMethods.First( m => m.Name == nameof(Resolve) && !m.IsPublic && m.IsStatic );

		public override void PreBuildUp( IBuilderContext context )
		{
			Guard.ArgumentNotNull( context, nameof(context) );
			if ( !context.HasBuildPlan() )
			{
				var type = context.BuildKey.Type.Adapt();
				if ( type.IsGenericOf<IEnumerable<object>>() )
				{
					var resolver = (Resolver)GenericResolveArrayMethod.MakeGenericMethod( type.GetEnumerableType() ).CreateDelegate( typeof(Resolver) );

					context.Existing = resolver( context );
					context.BuildComplete = true;
				}
			}
		}

		static object Resolve<T>( IBuilderContext context )
		{
			var result = context.Policies.Get<IRegisteredNamesPolicy>( null )
				.With( policy => policy.GetRegisteredNames( typeof(T) )
					.Concat( new string[] { null } ).Concat( typeof(T).GetTypeInfo().IsGenericType ? policy.GetRegisteredNames( typeof(T).GetGenericTypeDefinition() ) : Enumerable.Empty<string>() )
					.Distinct()
					.Select( context.NewBuildUp<T> )
					.ToArray() 
				) ?? new T[0];
			return result;
		}
	}
}
