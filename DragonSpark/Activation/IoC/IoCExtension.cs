using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Aspects;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.IoC
{
	public class IoCExtension : UnityContainerExtension, IDisposable
	{
		readonly MonitorLoggerCommand command = new MonitorLoggerCommand();

		class BuildKeyMonitorStrategy : BuilderStrategy
		{
			readonly IList<NamedTypeBuildKey> keys = new List<NamedTypeBuildKey>();

			public IEnumerable<NamedTypeBuildKey> Purge() => keys.Purge();

			public override void PreBuildUp( IBuilderContext context ) => keys.Ensure( context.BuildKey );
		}

		protected override void Initialize()
		{
			Context.RegisteringInstance += ContextOnRegisteringInstance;

			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );

			Context.Strategies.Clear();
			Context.Strategies.AddNew<BuildKeyMappingStrategy>( UnityBuildStage.TypeMapping );
			Context.Strategies.AddNew<HierarchicalLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<LifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<ConventionStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<ArrayResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<BuildPlanStrategy>( UnityBuildStage.Creation );

			var monitor = new BuildKeyMonitorStrategy();
			Context.BuildPlanStrategies.Add( monitor, UnityBuildStage.Setup );

			Container.Registration<EnsuredRegistrationSupport>().With( support =>
			{
				support.Instance( Logger );
				support.Instance<IResolutionSupport>( new ResolutionSupport( Context ) );
				support.Instance( CreateActivator );
				support.Instance( CreateRegistry );
			} );

			monitor.Purge().Each( Context.Policies.Clear<IBuildPlanPolicy> );

			Context.BuildPlanStrategies.Clear();
			Context.BuildPlanStrategies.AddNew<DynamicMethodConstructorStrategy>( UnityBuildStage.Creation );
			Context.BuildPlanStrategies.AddNew<DynamicMethodPropertySetterStrategy>( UnityBuildStage.Initialization );
			Context.BuildPlanStrategies.AddNew<DynamicMethodCallStrategy>( UnityBuildStage.Initialization );

			var policy = Context.Policies.Get<IBuildPlanCreatorPolicy>( null );
			var builder = new Builder<TryContext>( Context.Strategies, policy.CreatePlan );
			Context.Policies.SetDefault<IBuildPlanCreatorPolicy>( new BuildPlanCreatorPolicy( builder.Create, Policies, policy ) );
		}

		public IMessageLogger Logger => command.Logger;
		
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

		class MonitorLoggerCommand : Command<IMessageLogger>
		{
			readonly ConditionMonitor monitor = new ConditionMonitor();

			public MonitorLoggerCommand() : this( new RecordingMessageLogger() ) {}

			MonitorLoggerCommand( [Required]RecordingMessageLogger logger )
			{
				Logger = logger;
			}

			public RecordingMessageLogger Logger { get; }

			public override bool CanExecute( IMessageLogger parameter )
			{
				var result = base.CanExecute( parameter ) && parameter != Logger && !monitor.IsApplied;
				return result;
			}

			protected override void OnExecute( IMessageLogger parameter ) => monitor.Apply( () =>
			{
				var messages = Logger.Purge();
				parameter.Information( $"A new logger of type '{parameter}' has been registered.  Purging existing logger with '{messages.Length}' messages and routing them through the new logger." );
				messages.Each( parameter.Log );
			} );
		}

		void ContextOnRegisteringInstance( object sender, RegisterInstanceEventArgs args )
		{
			var type = args.Instance.GetType();

			var register = args.Instance.AsTo<IMessageLogger, bool>( logger => command.ExecuteWith( logger ) != null ) || !Container.IsRegistered( type, args.Name );

			if ( args.RegisteredType != type && register )
			{
				Container.Registration().Instance( type, args.Instance, args.Name, (LifetimeManager)Container.Resolve( args.LifetimeManager.GetType() ) );
			}
		}

		IServiceRegistry CreateRegistry() => new ServiceRegistry( Container );

		IActivator CreateActivator() => new CompositeActivator( Container.Resolve<Activator>(), SystemActivator.Instance );

		void IDisposable.Dispose() => Remove();
	}

	public class ConventionCandidateLocator : FactoryWithSpecification<IBuilderContext, Type>
	{
		public ConventionCandidateLocator( [Required]BuildableTypeFromConventionLocator factory ) : this( InvalidBuildFromContextSpecification.Instance, factory ) {}

		protected ConventionCandidateLocator( [Required]InvalidBuildFromContextSpecification specification, [Required]BuildableTypeFromConventionLocator factory ) : base( specification, context => factory.Create( context.BuildKey.Type ) ) {}
	}

	public class BuildableTypeFromConventionLocator : FactoryBase<Type, Type>
	{
		public static BuildableTypeFromConventionLocator Instance { get; } = new BuildableTypeFromConventionLocator();

		readonly Assembly[] assemblies;
		readonly CanBuildSpecification specification;

		public BuildableTypeFromConventionLocator() : this( Default<Assembly>.Items ) {}

		public BuildableTypeFromConventionLocator( [Required]Assembly[] assemblies ) : this( assemblies, CanBuildSpecification.Instance ) {}

		public BuildableTypeFromConventionLocator( [Required]Assembly[] assemblies, [Required]CanBuildSpecification specification )
		{
			this.assemblies = assemblies;
			this.specification = specification;
		}

		protected override Type CreateItem( Type parameter )
		{
			var adapter = parameter.Adapt();
			var name = parameter.Name.TrimStart( 'I' );
			var locations = assemblies.Any() ? assemblies : parameter.Assembly().ToItem();
			var result = locations.SelectMany( assembly => assembly.DefinedTypes.AsTypes() ).Where( adapter.IsAssignableFrom ).Where( specification.IsSatisfiedBy ).FirstOrDefault( candidate => candidate.Name.StartsWith( name ) );
			return result;
		}
	}

	public class ImplementedInterfaceFromConventionLocator : FactoryBase<Type,Type>
	{
		readonly Assembly[] assemblies;

		public ImplementedInterfaceFromConventionLocator( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Type CreateItem( Type parameter )
		{
			var locations = assemblies.Any() ? assemblies : parameter.Assembly().ToItem();
			var result =
				parameter.GetTypeInfo().ImplementedInterfaces.ToArray().With( interfaces => 
					interfaces.FirstOrDefault( i => parameter.Name.Contains( i.Name.TrimStart( 'I' ) ) )
					??
					interfaces.FirstOrDefault( t => locations.Contains( t.Assembly() ) )
				) ?? parameter;
			return result;
		}
	}

	public class CanBuildSpecification : SpecificationBase<Type>
	{
		public static CanBuildSpecification Instance { get; } = new CanBuildSpecification();

		protected override bool IsSatisfiedByParameter( Type parameter )
		{
			var info = parameter.GetTypeInfo();
			var result = base.IsSatisfiedByParameter( parameter ) && !info.IsInterface && !info.IsAbstract && !typeof(Delegate).Adapt().IsAssignableFrom( parameter );
			return result;
		}
	}

	public class InvalidBuildFromContextSpecification : SpecificationBase<IBuilderContext>
	{
		public static InvalidBuildFromContextSpecification Instance { get; } = new InvalidBuildFromContextSpecification();

		readonly CanBuildSpecification specification;

		public InvalidBuildFromContextSpecification() : this( CanBuildSpecification.Instance ) {}

		public InvalidBuildFromContextSpecification( [Required]CanBuildSpecification specification )
		{
			this.specification = specification;
		}

		protected override bool IsSatisfiedByParameter( IBuilderContext parameter )
		{
			var result = !base.IsSatisfiedByParameter( parameter ) || !specification.IsSatisfiedBy( parameter.BuildKey.Type ) || !CanBuildFrom( parameter );
			return result;
		}

		static bool CanBuildFrom( IBuilderContext parameter )
		{
			IPolicyList containingPolicyList;
			var constructor = parameter.Policies.Get<IConstructorSelectorPolicy>( parameter.BuildKey, out containingPolicyList).SelectConstructor(parameter, containingPolicyList);
			var result = constructor.With( IsValidConstructor );
			return result;
		}

		static bool IsValidConstructor( SelectedConstructor selectedConstructor ) => selectedConstructor.Constructor.GetParameters().All( pi => !pi.ParameterType.IsByRef );
	}

	public class ConventionStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			var chain = Ambient.GetCurrentChain<NamedTypeBuildKey>();
			var all = !chain.Select( key => key.Type ).Contains( typeof(ConventionCandidateLocator) );
			if ( all )
			{
				var locator = context.New<ConventionCandidateLocator>();
				var type = locator.Create( context );
				type.With( located =>
				{
					context.BuildKey = new NamedTypeBuildKey( located, context.BuildKey.Name );
				} );
			}
		}
	}
}