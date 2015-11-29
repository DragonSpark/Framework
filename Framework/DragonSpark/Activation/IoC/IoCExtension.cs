using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using LifetimeManagerFactory = DragonSpark.Setup.Registration.LifetimeManagerFactory;

namespace DragonSpark.Activation.IoC
{
	public class IoCExtension : UnityContainerExtension
	{
		public ILogger Logger { get; } = new RecordingLogger();
		
		protected override void Initialize()
		{
			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );

			Context.Strategies.Clear();
			Context.Strategies.AddNew<BuildKeyMappingStrategy>( UnityBuildStage.TypeMapping );
			Context.Strategies.AddNew<HierarchicalLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<LifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<ArrayResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<BuildPlanStrategy>( UnityBuildStage.Creation );
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
				lifetimeEntries.Each( container.Remove );
			}
		}

		public override void Remove()
		{
			Context.ChildContainerCreated -= ContextChildContainerCreated;
		}*/

		/*public IUnityContainer Dispose()
		{
			var entries = GetLifetimeEntries().Where( x => x.Value == Container ).Select( x => x.Key ).ToArray();
			entries.Each( Context.Lifetime.Remove );
			//Children.ToArray().Each( y => y.DisposeAll() );
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