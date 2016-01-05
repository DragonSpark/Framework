using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace DragonSpark.Activation.IoC
{
	public class IoCExtension : UnityContainerExtension, IDisposable
	{
		public IMessageLogger MessageLogger { get; } = new RecordingMessageLogger();

		protected override void Initialize()
		{
			Context.RegisteringInstance += ContextOnRegisteringInstance;

			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );

			Context.Strategies.Clear();
			Context.Strategies.AddNew<BuildChainMonitorStrategy>( UnityBuildStage.Setup );
			Context.Strategies.AddNew<BuildKeyMappingStrategy>( UnityBuildStage.TypeMapping );
			Context.Strategies.AddNew<HierarchicalLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<LifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<ArrayResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<BuildPlanStrategy>( UnityBuildStage.Creation );

			Context.Policies.SetDefault<IBuildPlanCreatorPolicy>( new BuildPlanCreatorPolicy( Policies, Context.Policies.Get<IBuildPlanCreatorPolicy>( null ) ) );

			var resolutionSupport = new ResolutionSupport( Context );
			Container.RegisterInstance<IResolutionSupport>( resolutionSupport );

			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( CreateActivator );
				support.Instance( CreateRegistry );
			});
		}

		public IList<IBuildPlanPolicy> Policies { get; } = new List<IBuildPlanPolicy> { new SingletonBuildPlanPolicy() };
		
		public override void Remove()
		{
			base.Remove();

			Context.RegisteringInstance -= ContextOnRegisteringInstance;
		}

		void ContextOnRegisteringInstance( object sender, RegisterInstanceEventArgs args )
		{
			var type = args.Instance.GetType();
			if ( args.RegisteredType != type && !Container.IsRegistered( type, args.Name ) )
			{
				Container.RegisterInstance( type, args.Name, args.Instance, (LifetimeManager)Container.Resolve( args.LifetimeManager.GetType() ) );
			}
		}

		IServiceRegistry CreateRegistry()
		{
			var factory = new Setup.Registration.LifetimeManagerFactory( Container.Resolve<IActivator>() );
			var result = new ServiceRegistry( Container, Container.DetermineLogger(), factory );
			return result;
		}

		IActivator CreateActivator() => new CompositeActivator( Container.Resolve<Activator>(), SystemActivator.Instance );

		public IUnityContainer Register( IServiceLocator locator ) => Container.Registration().Instance( locator );

		void IDisposable.Dispose() => Remove();
	}
}