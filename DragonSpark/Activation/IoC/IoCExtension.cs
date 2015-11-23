using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	public class DefaultUnityConstructorSelectorPolicy : Microsoft.Practices.Unity.ObjectBuilder.DefaultUnityConstructorSelectorPolicy
	{
		protected override IDependencyResolverPolicy CreateResolver( ParameterInfo parameter )
		{
			var isOptional = parameter.IsOptional && !parameter.IsDefined( typeof(OptionalDependencyAttribute) );
			var dependencyResolverPolicy = base.CreateResolver( parameter );
			var result = isOptional ? parameter.ParameterType.GetTypeInfo().IsValueType || parameter.ParameterType == typeof(string) ? (IDependencyResolverPolicy)new LiteralValueDependencyResolverPolicy( parameter.DefaultValue ) : new OptionalDependencyResolverPolicy( parameter.ParameterType ) : dependencyResolverPolicy;
			return result;
		}
	}

	/*public class SingletonStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			if ( !context.BuildComplete && context.Existing == null && context.BuildKey.Type.GetTypeInfo().IsInterface && !context.IsRegistered( context.BuildKey ) )
			{
				var locator = context.IsRegistered<ISingletonLocator>() ? context.NewBuildUp<ISingletonLocator>() : SingletonLocator.Instance;
				context.Existing = locator.Locate( context.BuildKey.Type );
				context.BuildComplete = context.Existing != null;
			}
		}
	}*/

	public class IoCExtension : UnityContainerExtension
	{
		public ILogger Logger { get; } = new RecordingLogger();
		/*internal IList<IDisposable> Disposables
		{
			get { return disposables.Value; }
		}	readonly Lazy<IList<IDisposable>> disposables = new Lazy<IList<IDisposable>>( () => new List<IDisposable>() );*/

		/*IList<IUnityContainer> Children => children.Value;
		readonly Lazy<IList<IUnityContainer>> children = new Lazy<IList<IUnityContainer>>( () => new List<IUnityContainer>() );*/

		protected override void Initialize()
		{
			Context.Container.EnsureRegistered( CreateActivator );

			Context.Policies.SetDefault<IConstructorSelectorPolicy>( new DefaultUnityConstructorSelectorPolicy() );

			//Context.Strategies.Add( CurrentBuildKeyStrategy, UnityBuildStage.Setup );
			// Context.Strategies.AddNew<CurrentContextStrategy>( UnityBuildStage.Setup );
			// Context.Strategies.AddNew<ApplicationConfigurationStrategy>( UnityBuildStage.PreCreation );
			/*Context.Strategies.AddNew<SingletonStrategy>( UnityBuildStage.PreCreation );*/
			// Context.Strategies.AddNew<ApplyBehaviorStrategy>( UnityBuildStage.Initialization );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );
			
			// Context.ChildContainerCreated += ContextChildContainerCreated;
		}

		public IActivator CreateActivator()
		{
			var result = new CompositeActivator( new Activator( Context ), SystemActivator.Instance );
			return result;
		}

		/*void ContextChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
		{
			e.ChildContainer.With( child =>
			{
				Children.Add( child );
				child.RegisterInstance( Container, new ContainerLifetimeManager() );
				child.RegisterInstance( new ChildContainerListener( this, child ) );
			} );
		}

		class ContainerLifetimeManager : LifetimeManager
		{
			object value;

			public override object GetValue()
			{
				return value;
			}

			public override void SetValue(object newValue)
			{
				value = newValue;
			}

			public override void RemoveValue()
			{}
		}

		class ChildContainerListener : IDisposable
		{
			readonly IoCExtension extension;
			readonly IUnityContainer child;

			public ChildContainerListener( IoCExtension extension, IUnityContainer child )
			{
				this.extension = extension;
				this.child = child;
			}

			void IDisposable.Dispose()
			{
				extension.Children.Remove( child );
				var container = extension.Context.Container.GetLifetimeContainer();
				var lifetimeEntries = child.GetLifetimeEntries().Where( x => x.Value == child );
				lifetimeEntries.Apply( container.Remove );
			}
		}

		public override void Remove()
		{
			Context.ChildContainerCreated -= ContextChildContainerCreated;
		}*/

		/*public IUnityContainer Dispose()
		{
			var entries = GetLifetimeEntries().Where( x => x.Value == Container ).Select( x => x.Key ).ToArray();
			entries.Apply( Context.Lifetime.Remove );
			//Children.ToArray().Apply( y => y.DisposeAll() );
			Container.Dispose();
			Container.RemoveAllExtensions();
			return Container;
		}*/

		/*public IEnumerable<LifetimeEntry> GetLifetimeEntries()
		{
			var result = Context.Lifetime.Select( ResolveLifetimeEntry ).ToArray();
			return result;
		}

		static LifetimeEntry ResolveLifetimeEntry( object x )
		{
			var result = x.AsTo<ILifetimePolicy, LifetimeEntry>( y => new LifetimeEntry( y, y.GetValue() ), () => new LifetimeEntry( x ) );
			return result;
		}*/
	}

	class Activator : IActivator
	{
		readonly ExtensionContext context;
		readonly IList<NamedTypeBuildKey> resolvable = new List<NamedTypeBuildKey>();

		public Activator( ExtensionContext context )
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
			var result = context.Container.IsRegistered( key.Type, key.Name ) && GetConstructor( key ) != null;
			return result;
		}

		ConstructorInfo GetConstructor( NamedTypeBuildKey key )
		{
			var mapped = context.Policies.Get<IBuildKeyMappingPolicy>( key ).Transform( policy => policy.Map( key, null ) ) ?? key;
			return context.Policies.Get<IConstructorSelectorPolicy>( mapped ).Transform( policy =>
			{
				var builder = new BuilderContext( context.BuildPlanStrategies.MakeStrategyChain(), context.Lifetime, context.Policies, mapped, null );
				try
				{
					var constructor = policy.SelectConstructor( builder, context.Policies );
					var result = constructor.Transform( selected => selected.Constructor ); 
					return result;
				}
				catch ( InvalidOperationException )
				{
					return null;
				}
			} );
		}

		bool IsRegistered( NamedTypeBuildKey key )
		{
			var result = CheckInstance( key ) || CheckRegistered( key );
			return result;
		}

		bool Validate( NamedTypeBuildKey key, IEnumerable<Type> parameters )
		{
			var result = CheckInstance( key ) || GetConstructor( key ).Transform( x => Validate( x, parameters ) );
			result.IsTrue( () => resolvable.Add( key ) );
			return result;
		}

		bool Validate( MethodBase constructor, IEnumerable<Type> parameters )
		{
			var result = constructor
				.GetParameters()
				.Select( parameterInfo => new NamedTypeBuildKey( parameterInfo.ParameterType ) )
				.All( key => parameters.Any( key.Type.IsAssignableFrom ) || IsRegistered( key ) );
			return result;
		}

		public bool CanActivate( Type type, string name = null )
		{
			return Check( type, name );
		}

		bool Check( Type type, string name, params object[] parameters )
		{
			var key = new NamedTypeBuildKey( type, name );
			var result = resolvable.Contains( key ) || Validate( key, parameters.NotNull().Select( o => o.GetType() ).ToArray() );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = context.Container.Resolve( type, name );
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			var result = Check( type, null, parameters );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			using ( var container = context.Container.CreateChildContainer() )
			{
				parameters.NotNull().Apply( x => 
				{
					x.As<TypedInjectionValue>( parameterValue =>
					{
						container.RegisterInstance( parameterValue.ParameterType, parameterValue.GetResolverPolicy( null ).Resolve( null ) );
					});
					x.GetType().GetTypeInfo().ImplementedInterfaces.Union( x.GetType().GetHierarchy( false ) ).Distinct().Apply( y => container.RegisterInstance( y, x ) );
				} );

				var result = new ResolutionContext( container.DetermineLogger() ).Execute( () => container.Resolve( type ) );
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
}