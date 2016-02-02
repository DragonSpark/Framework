using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Diagnostics;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.IoC
{
	public class BuildPlanCreatorPolicy : IBuildPlanCreatorPolicy
	{
		readonly Func<IBuilderContext, TryContext> builder;
		readonly IList<IBuildPlanPolicy> policies;
		readonly IBuildPlanCreatorPolicy[] creators;

		public BuildPlanCreatorPolicy( [Required]Func<IBuilderContext, TryContext> builder, [Required]IList<IBuildPlanPolicy> policies, [Required]params IBuildPlanCreatorPolicy[] creators )
		{
			this.builder = builder;
			this.policies = policies;
			this.creators = creators;
		}

		public IBuildPlanPolicy CreatePlan( IBuilderContext context, NamedTypeBuildKey buildKey ) => 
			new CompositeBuildPlanPolicy( builder, creators.Select( policy => policy.CreatePlan( context, buildKey ) ).Concat( policies ).ToArray() );
	}

	public class CompositeBuildPlanPolicy : IBuildPlanPolicy
	{
		readonly Func<IBuilderContext, TryContext> builder;
		readonly IBuildPlanPolicy[] policies;

		public CompositeBuildPlanPolicy( [Required]Func<IBuilderContext, TryContext> builder, params IBuildPlanPolicy[] policies )
		{
			this.builder = builder;
			this.policies = policies;
		}

		public void BuildUp( IBuilderContext context )
		{
			var @try = new Func<Action, Exception>( builder( context ).Try );
			Exception first = null;
			foreach ( var exception in policies.Select( policy => @try( () => policy.BuildUp( context ) ) ) )
			{
				if ( exception == null && context.Existing != null )
				{
					return;
				}
				first = first ?? exception;
			}
			throw first;
		}
	}

	class SingletonBuildPlanPolicy : IBuildPlanPolicy
	{
		public void BuildUp( IBuilderContext context )
		{
			var locator = context.New<ISingletonLocator>();
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

		public override void PreBuildUp( [Required]IBuilderContext context )
		{
			if ( !context.HasBuildPlan() )
			{
				var adapt = context.BuildKey.Type.Adapt();
				if ( adapt.IsGenericOf<IEnumerable<object>>( false ) )
				{
					adapt.GetEnumerableType().With( type =>
					{
						var resolver = (Resolver)GenericResolveArrayMethod.MakeGenericMethod( type ).CreateDelegate( typeof(Resolver) );

						context.Existing = resolver( context );
						context.BuildComplete = true;
					} );
				}
			}
		}

		static object Resolve<T>( IBuilderContext context )
		{
			var result = context.Policies.Get<IRegisteredNamesPolicy>( null )
				.With( policy => policy.GetRegisteredNames( typeof(T) )
					.Concat( new string[] { null } ).Concat( typeof(T).GetTypeInfo().IsGenericType ? policy.GetRegisteredNames( typeof(T).GetGenericTypeDefinition() ) : Enumerable.Empty<string>() )
					.Distinct()
					.Select( context.New<T> )
					.ToArray() 
				) ?? new T[0];
			return result;
		}
	}
}
