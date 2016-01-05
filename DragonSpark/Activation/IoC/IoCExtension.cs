using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.IoC
{
	[BuildUp]
	public class IoCExtension : UnityContainerExtension, IDisposable
	{
		[Activate( typeof(RecordingMessageLogger) )]
		public IMessageLogger Logger { get; set; }

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

			var resolutionSupport = new ResolutionSupport( Context );
			Container.RegisterInstance<IResolutionSupport>( resolutionSupport );

			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( CreateActivator );
				support.Instance( CreateRegistry );
				support.Instance( Logger );
			} );

			var policy = Context.Policies.Get<IBuildPlanCreatorPolicy>( null );
			var builder = new Builder<TryContext>( Context.Strategies, policy.CreatePlan );
			Context.Policies.SetDefault<IBuildPlanCreatorPolicy>( new BuildPlanCreatorPolicy( builder.Create, Policies, policy ) );
		}

		public class Builder<T> : FactoryBase<IBuilderContext, T>
		{
			readonly NamedTypeBuildKey key = NamedTypeBuildKey.Make<T>();
			readonly IStagedStrategyChain strategies;
			readonly Func<IBuilderContext, NamedTypeBuildKey, IBuildPlanPolicy> creator;

			public Builder( [Required]IStagedStrategyChain strategies, [Required]Func<IBuilderContext, NamedTypeBuildKey, IBuildPlanPolicy> creator )
			{
				this.strategies = strategies;
				this.creator = creator;
			}

			protected override T CreateItem( IBuilderContext parameter )
			{
				var context = new BuilderContext( strategies.MakeStrategyChain(), parameter.Lifetime, parameter.PersistentPolicies, parameter.Policies, key, null );
				var plan = creator( context, key );
				plan.BuildUp( context );
				var result = context.Existing.To<T>();
				return result;
			}
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
			var result = new ServiceRegistry( Container, Container.Logger(), factory );
			return result;
		}

		IActivator CreateActivator() => new CompositeActivator( Container.Resolve<Activator>(), SystemActivator.Instance );

		public IUnityContainer Register( IServiceLocator locator ) => Container.Registration().Instance( locator );

		void IDisposable.Dispose() => Remove();
	}
}