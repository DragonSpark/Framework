using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
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
			Context.Container.RegisterInstance<IResolutionSupport>( new ResolutionSupport( Context ) );

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