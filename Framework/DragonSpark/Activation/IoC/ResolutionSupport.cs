using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	class ResolutionSupport : IResolutionSupport
	{
		readonly ExtensionContext context;
		readonly IList<NamedTypeBuildKey> resolvable = new List<NamedTypeBuildKey>();

		public ResolutionSupport( ExtensionContext context )
		{
			this.context = context;
		}

		bool CheckInstance( NamedTypeBuildKey key )
		{
			var result = context.Policies.Get<ILifetimePolicy>( key ).With( policy => policy.GetValue() ) != null;
			return result;
		}

		bool CheckRegistered( NamedTypeBuildKey key )
		{
			var result = context.Container.IsRegistered( key.Type, key.Name ) && !( context.Policies.GetNoDefault<IBuildPlanPolicy>( key, false ) is DynamicMethodBuildPlan );
			return result;
		}

		ConstructorInfo GetConstructor( NamedTypeBuildKey key )
		{
			var mapped = context.Policies.Get<IBuildKeyMappingPolicy>( key ).With( policy => policy.Map( key, null ) ) ?? key;
			return context.Policies.Get<IConstructorSelectorPolicy>( mapped ).With( policy =>
			{
				var builder = new BuilderContext( context.BuildPlanStrategies.MakeStrategyChain(), context.Lifetime, context.Policies, mapped, null );
				var constructor = policy.SelectConstructor( builder, context.Policies );
				var result = constructor.With( selected => selected.Constructor ); 
				return result;
			} );
		}

		bool IsRegistered( NamedTypeBuildKey key )
		{
			var result = CheckInstance( key ) || CheckRegistered( key );
			return result;
		}

		bool Validate( NamedTypeBuildKey key, IEnumerable<Type> parameters )
		{
			var result = IsRegistered( key ) || GetConstructor( key ).With( x => Validate( x, parameters ) );
			result.IsTrue( () => resolvable.Add( key ) );
			return result;
		}

		bool Validate( MethodBase constructor, IEnumerable<Type> parameters )
		{
			var result = constructor
				.GetParameters()
				.Where( x => !IntrospectionExtensions.GetTypeInfo( x.ParameterType ).IsValueType )
				.Select( parameterInfo => new NamedTypeBuildKey( parameterInfo.ParameterType ) )
				.All( key => parameters.Any( key.Type.Extend().IsAssignableFrom ) || IsRegistered( key ) );
			return result;
		}

		public bool CanResolve( Type type, string name, params object[] parameters )
		{
			var key = new NamedTypeBuildKey( type, name );
			var result = resolvable.Contains( key ) || Validate( key, parameters.NotNull().Select( o => o.GetType() ).ToArray() );
			return result;
		}
	}
}