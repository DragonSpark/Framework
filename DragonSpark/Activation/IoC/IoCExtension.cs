using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Reflection;
using LifetimeManagerFactory = DragonSpark.Setup.LifetimeManagerFactory;

namespace DragonSpark.Activation.IoC
{
	public class DefaultUnityConstructorSelectorPolicy : Microsoft.Practices.Unity.ObjectBuilder.DefaultUnityConstructorSelectorPolicy
	{
		public static DefaultUnityConstructorSelectorPolicy Instance { get; } = new DefaultUnityConstructorSelectorPolicy();

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

	public class ServiceRegistry : IServiceRegistry
	{
		readonly IUnityContainer container;
		readonly ILogger logger;
		readonly IFactory<Type, LifetimeManager> lifetimeFactory;

		/*public ServiceRegistry( IUnityContainer container ) : this( container, LifetimeManagerFactory.Instance )
		{}

		public ServiceRegistry( IUnityContainer container, IFactory<Type, LifetimeManager> lifetimeFactory ) : this( container, container.DetermineLogger(), lifetimeFactory )
		{
		}*/

		public ServiceRegistry( IUnityContainer container, ILogger logger, IFactory<Type, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.logger = logger;
			this.lifetimeFactory = lifetimeFactory;
		}

		public void Register( Type @from, Type mappedTo, string name = null )
		{
			var lifetimeManager = lifetimeFactory.Create( mappedTo );
			logger.Information( string.Format( Resources.UnityConventionRegistrationService_Registering, @from, mappedTo ) );
			container.RegisterType( from, mappedTo, name, lifetimeManager );
		}

		public void Register( Type type, object instance )
		{
			container.RegisterInstance( type, instance );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			container.RegisterType( type, new InjectionFactory( x =>
			{
				var item = factory();
				return item;
			} ) );
		}
	}

	public class IoCExtension : UnityContainerExtension
	{
		public ILogger Logger { get; } = new RecordingLogger();
		
		protected override void Initialize()
		{
			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );

			//Context.Strategies.Add( CurrentBuildKeyStrategy, UnityBuildStage.Setup );
			// Context.Strategies.AddNew<CurrentContextStrategy>( UnityBuildStage.Setup );
			// Context.Strategies.AddNew<ApplicationConfigurationStrategy>( UnityBuildStage.PreCreation );
			/*Context.Strategies.AddNew<SingletonStrategy>( UnityBuildStage.PreCreation );*/
			// Context.Strategies.AddNew<ApplyBehaviorStrategy>( UnityBuildStage.Initialization );
			// Context.ChildContainerCreated += ContextChildContainerCreated;

			Container.RegisterInstance<IResolutionSupport>( new ResolutionSupport( Context ) );
			Container.EnsureRegistered( CreateActivator );
			Container.EnsureRegistered( CreateRegistry  );
		}

		IServiceRegistry CreateRegistry()
		{
			var factory = new LifetimeManagerFactory( Container.Resolve<IActivator>() );
			var result = new ServiceRegistry( Container, Container.DetermineLogger(), factory );
			return result;
		}

		public IActivator CreateActivator()
		{
			var result = new CompositeActivator( Container.Resolve<Activator>(), SystemActivator.Instance );
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
}