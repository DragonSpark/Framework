using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using CompositeActivator = System.Composition.Hosting.Core.CompositeActivator;
using Type = System.Type;

namespace DragonSpark.Composition
{
	public abstract class SourceDelegateExporterBase : ExportDescriptorProviderBase
	{
		readonly static IStackSource<CompositeActivatorParameters> Stack = new AmbientStack<CompositeActivatorParameters>();
		readonly static Func<Type, Type> Locator = SourceTypes.Default.ToDelegate();
		readonly static Func<Type, Type> Parameters = ParameterTypes.Default.ToDelegate();

		readonly ICache<LifetimeContext, object> cache = new Cache<LifetimeContext, object>();
		readonly Func<ActivationParameter, object> resultSource;
		readonly Func<CompositionContract, CompositionContract> resolver;

		protected SourceDelegateExporterBase( Func<ActivationParameter, object> resultSource, Func<CompositionContract, CompositionContract> resolver )
		{
			this.resultSource = resultSource;
			this.resolver = resolver;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			CompositionDependency dependency;
			var exists = descriptorAccessor.TryResolveOptionalDependency( "Existing Request", contract, true, out dependency );
			if ( !exists )
			{
				var resolved = resolver( contract );
				var sourceType = resolved
					.With( compositionContract => compositionContract.ContractType )
					.With( Locator );
				var success = sourceType != null && descriptorAccessor.TryResolveOptionalDependency( "Source Request", contract.ChangeType( sourceType ), true, out dependency );
				if ( success )
				{
					IDictionary<CompositionContract, CompositeActivator> registry = new ConcurrentDictionary<CompositionContract, CompositeActivator>();
					Register( registry, descriptorAccessor, new[] { sourceType, Parameters( sourceType ) }.WhereAssigned().Select( resolved.ChangeType ).ToArray() );
					var activator = new Activator( Stack, new Source( registry.TryGet, resolved ) );
					var source = new ActivatorSource( Stack, resultSource, new ActivationParameter( activator, sourceType ) );
					var factory = dependency.Target.IsShared ? new SharedFactory( source.Create, cache, Contracts.Default.Get( dependency.Contract ) ) : new Factory( source.Create );
					yield return new ExportDescriptorPromise( dependency.Contract, GetType().Name, dependency.Target.IsShared, NoDependencies, factory.Get );
				}
			}
		}

		void Register( IDictionary<CompositionContract, CompositeActivator> registry, DependencyAccessor accessor, params CompositionContract[] contracts )
		{
			foreach ( var contract in contracts )
			{
				CompositionDependency dependency;
				if ( accessor.TryResolveOptionalDependency( $"Activator Request for '{GetType().FullName}'", contract, true, out dependency ) )
				{
					registry.Add( contract, dependency.Target.GetDescriptor().Activator );
				}
			}
		}

		class Factory : FactoryBase
		{
			readonly CompositeActivator activator;

			public Factory( CompositeActivator activator )
			{
				this.activator = activator;
			}

			protected override object Create( LifetimeContext context, CompositionOperation operation ) => context.Registered( activator( context, operation ) );
		}

		sealed class SharedFactory : Factory
		{
			readonly ICache<LifetimeContext, object> cache;
			readonly string boundary;

			public SharedFactory( CompositeActivator activator, ICache<LifetimeContext, object> cache, string boundary = null ) : base( activator )
			{
				this.cache = cache;
				this.boundary = boundary;
			}

			protected override object Create( LifetimeContext context, CompositionOperation operation )
			{
				var key = context.FindContextWithin( boundary );
				var result = cache.Get( key ) ?? cache.SetValue( key, base.Create( key, operation ) );
				return result;
			}
		}

		sealed class Source : ParameterizedSourceBase<Type, CompositeActivator>
		{
			readonly Func<CompositionContract, CompositeActivator> provider;
			readonly CompositionContract source;

			public Source( Func<CompositionContract, CompositeActivator> provider, CompositionContract source )
			{
				this.provider = provider;
				this.source = source;
			}

			public override CompositeActivator Get( Type parameter ) => provider( source.ChangeType( parameter ) );
		}

		sealed class Activator : ActivatorBase
		{
			public Activator( IStackSource<CompositeActivatorParameters> stack, IParameterizedSource<Type, CompositeActivator> source ) : base( new Specification( source ), new Inner( stack, source ).Get ) {}
			
			sealed class Specification : DelegatedAssignedSpecification<Type, CompositeActivator>
			{
				public Specification( IParameterizedSource<Type, CompositeActivator> source ) : base( source.ToDelegate() ) {}
			}

			sealed class Inner : ParameterizedSourceBase<Type, object>
			{
				readonly IStackSource<CompositeActivatorParameters> stack;
				readonly IParameterizedSource<Type, CompositeActivator> source;

				public Inner( IStackSource<CompositeActivatorParameters> stack, IParameterizedSource<Type, CompositeActivator> source  )
				{
					this.stack = stack;
					this.source = source;
				}

				public override object Get( Type parameter )
				{
					var activator = source.Get( parameter );
					var current = stack.GetCurrentItem();
					var result = activator( current.Context, current.Operation );
					return result;
				}
			}
		}
	}
}