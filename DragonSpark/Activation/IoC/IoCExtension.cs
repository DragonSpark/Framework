using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
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
		// readonly ConditionMonitor initialize = new ConditionMonitor();
		/*internal IList<IDisposable> Disposables
		{
			get { return disposables.Value; }
		}	readonly Lazy<IList<IDisposable>> disposables = new Lazy<IList<IDisposable>>( () => new List<IDisposable>() );*/

		internal IList<IUnityContainer> Children => children.Value;
		readonly Lazy<IList<IUnityContainer>> children = new Lazy<IList<IUnityContainer>>( () => new List<IUnityContainer>() );

		internal ILifetimeContainer LifetimeContainer => Context.Lifetime;

		// CurrentBuildKeyStrategy CurrentBuildKeyStrategy { get; set; }

		protected override void Initialize()
		{
			Context.Container.EnsureRegistered<IActivator>( () => new CompositeActivator( new Activator( Container ), SystemActivator.Instance ) );

			/*Container.EnsureRegistered<ILogger, DebugLogger>();
			Container.EnsureRegistered<IExceptionFormatter, ExceptionFormatter>();*/

			// CurrentBuildKeyStrategy = new CurrentBuildKeyStrategy();

			Context.Policies.SetDefault<IConstructorSelectorPolicy>( new DefaultUnityConstructorSelectorPolicy() );

			//Context.Strategies.Add( CurrentBuildKeyStrategy, UnityBuildStage.Setup );
			// Context.Strategies.AddNew<CurrentContextStrategy>( UnityBuildStage.Setup );

			// Context.Strategies.AddNew<ApplicationConfigurationStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.PreCreation );
			/*Context.Strategies.AddNew<SingletonStrategy>( UnityBuildStage.PreCreation );*/
			Context.Strategies.AddNew<DefaultValueStrategy>( UnityBuildStage.Initialization );
			
			// Context.Strategies.AddNew<ApplyBehaviorStrategy>( UnityBuildStage.Initialization );
			
			/*Context.RegisteringInstance += ContextRegisteringInstance;
			Context.Registering += ContextRegistering;*/
			Context.ChildContainerCreated += ContextChildContainerCreated;
		}

		void ContextChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
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
				var lifetimeEntries = child.GetLifetimeEntries().Where( x => x.Value == this );
				lifetimeEntries.Apply( container.Remove );
			}
		}

		public override void Remove()
		{
			/*Context.RegisteringInstance -= ContextRegisteringInstance;
			Context.Registering -= ContextRegistering;*/
			Context.ChildContainerCreated -= ContextChildContainerCreated;
		}

		public object Create( Type type, object[] parameters )
		{
			using ( var container = Context.Container.CreateChildContainer() )
			{
				using ( new CreationContext( container, parameters ) )
				{
					var result = container.Resolve( type );
					return result;	
				}
			}
		}

		class CreationContext : IDisposable
		{
			readonly IUnityContainer container;
			readonly IServiceLocator current;
			readonly object[] values;

			readonly IList<LifetimeManager> managers = new List<LifetimeManager>(); 

			public CreationContext( IUnityContainer container, IEnumerable<object> parameters )
			{
				this.container = container;

				values = parameters.NotNull().Select( x => 
				{
					var parameterValue = x.As<TypedInjectionValue>();
					if ( parameterValue != null )
					{
						var instance = parameterValue.GetResolverPolicy( null ).Resolve( null );
						Register( parameterValue.ParameterType, instance );
						return instance;
					}
					x.GetType().GetTypeInfo().ImplementedInterfaces.Union( x.GetType().GetHierarchy( false ) ).Distinct().Apply( y => Register( y, x ) );
					return x;
				} ).ToArray();

				current = Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet ? Microsoft.Practices.ServiceLocation.ServiceLocator.Current : null;

				var locator = new UnityServiceLocator( container );
				Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => locator );
			}

			IUnityContainer Register( Type type, object instance )
			{
				var manager = new ContainerControlledLifetimeManager().With( managers.Add );
				return container.RegisterInstance( type, instance, manager );
			}

			public void Dispose()
			{
				current.With( x => Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider( () => x ) );
				
				var lifetime = container.GetLifetimeContainer();
				var entries = container.GetLifetimeEntries().Where( x => values.Contains( x.Value ) ).ToArray();
				entries.Apply( x => lifetime.Remove( x.Key ) );
				managers.Apply( x => x.RemoveValue() );
				managers.Clear();
			}
		}

		/*internal TResult Create<TResult>( object[] parameters )
		{
			var result = Create( typeof(TResult), parameters ).To<TResult>();
			return result;
		}*/

		/*public void Complete()
		{
			initialize.Apply( () =>
			{
				/* Context.Container.AddExtension( ConfigurationExtension.Extension ) #1#
			} );
		}*/

		readonly IList<NamedTypeBuildKey> resolvable = new List<NamedTypeBuildKey>();

		bool CheckInstance( NamedTypeBuildKey key )
		{
			var result = Context.Policies.Get<ILifetimePolicy>( key ).Transform( policy => policy.GetValue() ) != null;
			return result;
		}

		bool CheckRegistered( NamedTypeBuildKey key )
		{
			var result = Container.IsRegistered( key.Type, key.Name ) && GetConstructor( key ) != null;
			return result;
		}

		ConstructorInfo GetConstructor( NamedTypeBuildKey key )
		{
			var mapped = Context.Policies.Get<IBuildKeyMappingPolicy>( key ).Transform( policy => policy.Map( key, null ) ) ?? key;
			return Context.Policies.Get<IConstructorSelectorPolicy>( mapped ).Transform( policy =>
			{
				var context = new BuilderContext( Context.BuildPlanStrategies.MakeStrategyChain(), Context.Lifetime, Context.Policies, mapped, null );
				var constructor = policy.SelectConstructor( context, Context.Policies );
				var result = constructor.Transform( selected => selected.Constructor ); 
				return result;
			} );
		}

		bool IsRegistered( NamedTypeBuildKey key )
		{
			var result = CheckInstance( key ) || CheckRegistered( key );
			return result;
		}

		bool Validate( NamedTypeBuildKey key )
		{
			var result = CheckInstance( key ) || GetConstructor( key ).Transform( Validate );
			result.IsTrue( () => resolvable.Add( key ) );
			return result;
		}

		bool Validate( ConstructorInfo constructor )
		{
			var result = constructor
				.GetParameters()
				.Select( parameterInfo => new NamedTypeBuildKey( parameterInfo.ParameterType ) )
				.All( IsRegistered );
			return result;
		}

		public bool CanResolve( Type type, string name )
		{
			var key = new NamedTypeBuildKey( type, name );
			var result = resolvable.Contains( key ) || Validate( key );
			return result;
		}

		public object Resolve( Type serviceType, string key )
		{
			var context = Container.IsResolvable<ContainerResolutionContext>() ? Container.Resolve<ContainerResolutionContext>() : new ContainerResolutionContext( Container, DetermineLogger() );
			var result = context.Resolve( serviceType, key );
			return result;
		}

		ILogger DetermineLogger()
		{
			var result = Container.IsRegistered<ILogger>() ? Container.Resolve<ILogger>() : Logger;
			return result;
		}
	}
}