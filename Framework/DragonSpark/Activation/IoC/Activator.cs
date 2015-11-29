using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	class Activator : IActivator
	{
		readonly IUnityContainer container;
		readonly IResolutionSupport support;

		public Activator( IUnityContainer container, IResolutionSupport support )
		{
			this.container = container;
			this.support = support;
		}

		public bool CanActivate( Type type, string name = null )
		{
			return support.CanResolve( type, name );
		}

		public object Activate( Type type, string name = null )
		{
			var result = container.Resolve( type, name );
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var result = support.CanResolve( type, null, parameters );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			using ( var child = container.CreateChildContainer() )
			{
				parameters.NotNull().Apply( x => 
				{
					x.As<TypedInjectionValue>( parameterValue =>
					{
						child.RegisterInstance( parameterValue.ParameterType, parameterValue.GetResolverPolicy( null ).Resolve( null ) );
					});

					child.RegisterAllClasses( x );
				} );

				var result = new ResolutionContext( child.DetermineLogger() ).Execute( () => child.Resolve( type ) );
				return result;
			}
		}
	}

	class ResolutionContext
	{
		readonly ILogger logger;

		public ResolutionContext( ILogger logger )
		{
			this.logger = logger;
		}

		public object Execute( Func<object> resolve )
		{
			try
			{
				var result = resolve();
				return result;
			}
			catch ( ResolutionFailedException e )
			{
				logger.Exception( string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None ), e );
				return null;
			}
		}
	}

	interface IResolutionSupport
	{
		bool CanResolve( Type type, string name, params object[] parameters );
	}

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
			var result = context.Policies.Get<ILifetimePolicy>( key ).Transform( policy => policy.GetValue() ) != null;
			return result;
		}

		bool CheckRegistered( NamedTypeBuildKey key )
		{
			var result = context.Container.IsRegistered( key.Type, key.Name ) && !( context.Policies.GetNoDefault<IBuildPlanPolicy>( key, false ) is DynamicMethodBuildPlan );
			return result;
		}

		ConstructorInfo GetConstructor( NamedTypeBuildKey key )
		{
			var mapped = context.Policies.Get<IBuildKeyMappingPolicy>( key ).Transform( policy => policy.Map( key, null ) ) ?? key;
			return context.Policies.Get<IConstructorSelectorPolicy>( mapped ).Transform( policy =>
			{
				var builder = new BuilderContext( context.BuildPlanStrategies.MakeStrategyChain(), context.Lifetime, context.Policies, mapped, null );
				var constructor = policy.SelectConstructor( builder, context.Policies );
				var result = constructor.Transform( selected => selected.Constructor ); 
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
			var result = IsRegistered( key ) || GetConstructor( key ).Transform( x => Validate( x, parameters ) );
			result.IsTrue( () => resolvable.Add( key ) );
			return result;
		}

		bool Validate( MethodBase constructor, IEnumerable<Type> parameters )
		{
			var result = constructor
				.GetParameters()
				.Where( x => !x.ParameterType.GetTypeInfo().IsValueType )
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