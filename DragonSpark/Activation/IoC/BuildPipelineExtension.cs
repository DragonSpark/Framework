using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Activation.IoC
{
	public class RegistrationMonitorExtension : UnityContainerExtension, IDisposable
	{
		// readonly MonitorLoggerCommand command = new MonitorLoggerCommand();

		protected override void Initialize() => Context.RegisteringInstance += ContextOnRegisteringInstance;

		void ContextOnRegisteringInstance( object sender, RegisterInstanceEventArgs args )
		{
			var type = args.Instance.GetType();

			if ( args.RegisteredType != type && !Container.IsRegistered( type, args.Name ) )
			{
				var registry = new ServiceRegistry( Container, args.LifetimeManager.GetType() );
				registry.Register( new InstanceRegistrationParameter( type, args.Instance, args.Name ) );
			}
		}

		void IDisposable.Dispose() => Remove();

		public override void Remove()
		{
			base.Remove();

			Context.RegisteringInstance -= ContextOnRegisteringInstance;
		}

		/*class MonitorLoggerCommand : Command<IMessageLogger>
		{
			readonly ConditionMonitor monitor = new ConditionMonitor();

			public MonitorLoggerCommand() : this( new RecordingMessageLogger() ) { }

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
		}*/
	}

	public class BuildPipelineExtension : UnityContainerExtension
	{
		class BuildKeyMonitorStrategy : BuilderStrategy
		{
			readonly IList<NamedTypeBuildKey> keys = new List<NamedTypeBuildKey>();

			public IEnumerable<NamedTypeBuildKey> Purge() => keys.Purge();

			public override void PreBuildUp( IBuilderContext context ) => keys.Ensure( context.BuildKey );
		}

		class MetadataLifetimeStrategy : BuilderStrategy
		{
			public override void PreBuildUp( IBuilderContext context )
			{
				var reference = new KeyReference( this, context.BuildKey ).Item;
				if ( new Checked( reference, this ).Item.Apply() )
				{
					var lifetimePolicy = context.Policies.GetNoDefault<ILifetimePolicy>( context.BuildKey, false );
					lifetimePolicy.Null( () =>
					{
						var factory = context.New<LifetimeManagerFactory>();
						var lifetimeManager = factory.Create( reference.Type );
						lifetimeManager.With( manager =>
						{
							context.New<IMessageLogger>().Information( $"'{GetType().Name}' is assigning a lifetime manager of '{manager.GetType()}' for type '{reference.Type}'." );

							context.PersistentPolicies.Set<ILifetimePolicy>( manager, reference );
						} );
					} );
				}
			}
		}

		protected override void Initialize()
		{
			Context.Policies.SetDefault<IConstructorSelectorPolicy>( DefaultUnityConstructorSelectorPolicy.Instance );

			Context.Strategies.Clear();
			Context.Strategies.AddNew<BuildKeyMappingStrategy>( UnityBuildStage.TypeMapping );
			Context.Strategies.AddNew<MetadataLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<HierarchicalLifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<LifetimeStrategy>( UnityBuildStage.Lifetime );
			Context.Strategies.AddNew<ArrayResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<EnumerableResolutionStrategy>( UnityBuildStage.Creation );
			Context.Strategies.AddNew<BuildPlanStrategy>( UnityBuildStage.Creation );

			var monitor = new BuildKeyMonitorStrategy();
			Context.BuildPlanStrategies.Add( monitor, UnityBuildStage.Setup );

			Context.New<PersistentServiceRegistry>().With( registry =>
			{
				registry.Register<IServiceRegistry, ServiceRegistry>();
				registry.Register<IActivator, Activator>();
				registry.Register( Context );
			} );

			monitor.Purge().Each( Context.Policies.Clear<IBuildPlanPolicy> );

			Context.BuildPlanStrategies.Clear();
			Context.BuildPlanStrategies.AddNew<DynamicMethodConstructorStrategy>( UnityBuildStage.Creation );
			Context.BuildPlanStrategies.AddNew<DynamicMethodPropertySetterStrategy>( UnityBuildStage.Initialization );
			Context.BuildPlanStrategies.AddNew<DynamicMethodCallStrategy>( UnityBuildStage.Initialization );

			Context.Strategies.AddNew<ConventionStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<FactoryStrategy>( UnityBuildStage.PreCreation );

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

		class Activator : CompositeActivator
		{
			public Activator( [Required]IoC.Activator activator ) : base( activator, SystemActivator.Instance ) {}
		}
	}

	public class BuildableTypeFromConventionLocator : FactoryBase<Type, Type>
	{
		readonly Assembly[] assemblies;
		readonly CanBuildSpecification specification;

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
			var result = assemblies.AnyOr( () => parameter.Assembly().ToItem() )
				.SelectMany( assembly => assembly.DefinedTypes.AsTypes() )
				.Where( adapter.IsAssignableFrom )
				.Where( specification.IsSatisfiedBy )
				.FirstOrDefault( candidate => candidate.Name.StartsWith( name ) );
			return result;
		}
	}

	public class ImplementedFromConventionTypeLocator : FactoryBase<Type, Type>
	{
		public static ImplementedFromConventionTypeLocator Instance { get; } = new ImplementedFromConventionTypeLocator();

		[Freeze]
		protected override Type CreateItem( Type parameter )
		{
			var assemblies = new Assemblies.Get[] { Assemblies.GetCurrent, parameter.Append( GetType() ).Distinct().Assemblies };

			var result = assemblies.FirstWhere( get => new ImplementedInterfaceFromConventionLocator( get() ).Create( parameter ) );
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
			var result =
				parameter.GetTypeInfo().ImplementedInterfaces.ToArray().With( interfaces => 
					interfaces.FirstOrDefault( i => parameter.Name.Contains( i.Name.TrimStart( 'I' ) ) )
					??
					interfaces.FirstOrDefault( t => assemblies.Contains( t.Assembly() ) )
				) ?? parameter;
			return result;
		}
	}

	public class CanBuildSpecification : SpecificationBase<Type>
	{
		public static CanBuildSpecification Instance { get; } = new CanBuildSpecification();

		protected override bool Verify( Type parameter )
		{
			var info = parameter.GetTypeInfo();
			var result = parameter != typeof(object) && !info.IsInterface && !info.IsAbstract && !typeof(Delegate).Adapt().IsAssignableFrom( parameter );
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

		protected override bool Verify( IBuilderContext parameter )
		{
			var result = !specification.IsSatisfiedBy( parameter.BuildKey.Type ) || !CanBuildFrom( parameter );
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

	class KeyReference : Reference<NamedTypeBuildKey>
	{
		public KeyReference( object instance, NamedTypeBuildKey key ) : base( instance, key ) { }
	}

	public class FactoryStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			var reference = new KeyReference( this, context.BuildKey ).Item;
			var hasBuildPlan = context.HasBuildPlan();
			if ( reference.Name == null && !hasBuildPlan && new Checked( reference, this ).Item.Apply() )
			{
				context.Existing = Factory.Create( context.BuildKey.Type );

				context.Existing.With( o =>
				{
					var registry = context.New<IServiceRegistry>();
					registry.Register( new InstanceRegistrationParameter( context.BuildKey.Type, o, context.BuildKey.Name ) );
				} );

				context.BuildComplete = context.Existing != null;
			}
		}
	}

	public class ConventionStrategy : BuilderStrategy
	{
		[Persistent]
		class ConventionCandidateLocator : SpecificationAwareFactory<IBuilderContext, Type>
		{
			public ConventionCandidateLocator( [Required]BuildableTypeFromConventionLocator factory ) : this( InvalidBuildFromContextSpecification.Instance, factory ) { }

			ConventionCandidateLocator( [Required]InvalidBuildFromContextSpecification specification, [Required]BuildableTypeFromConventionLocator factory ) : base( specification, context => factory.Create( context.BuildKey.Type ) ) { }
		}

		public override void PreBuildUp( IBuilderContext context )
		{
			var reference = new KeyReference( this, context.BuildKey ).Item;
			if ( new Checked( reference, this ).Item.Apply() )
			{
				context.New<ConventionCandidateLocator>().Create( context ).With( located =>
				{
					var mapped = new NamedTypeBuildKey( located, context.BuildKey.Name );

					var registry = context.New<IServiceRegistry>();

					registry.Register( new MappingRegistrationParameter( context.BuildKey.Type, mapped.Type, context.BuildKey.Name ) );

					context.BuildKey = mapped;
				} );
			}
		}
	}
}