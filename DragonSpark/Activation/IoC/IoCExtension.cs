﻿using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.Activation.IoC
{
	public class ObjectBuilderExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );
			Context.Policies.Set<IObjectBuilderPolicy>( new ObjectBuilderPolicy( false ), typeof(IObjectBuilder) );
			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance<IObjectBuilder>( ObjectBuilder.Instance );
			});
		}

		public void Enable( bool on )
		{
			Context.Policies.SetDefault<IObjectBuilderPolicy>( new ObjectBuilderPolicy( on ) );
		}
	}

	public class IoCExtension : UnityContainerExtension
	{
		public IMessageLogger MessageLogger { get; } = new RecordingMessageLogger();

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
			
			Container.RegisterInstance<IResolutionSupport>( new ResolutionSupport( Context ) );

			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( CreateActivator );
				support.Instance( CreateRegistry );
			});
		}

		IServiceRegistry CreateRegistry()
		{
			var factory = new Setup.Registration.LifetimeManagerFactory( Container.Resolve<IActivator>() );
			var result = new ServiceRegistry( Container, Container.DetermineLogger(), factory );
			return result;
		}

		IActivator CreateActivator()
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

		public IUnityContainer Register( IServiceLocator locator )
		{
			return Container.Registration().Instance( locator );
		}
	}
}