using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using LifetimeManager = Microsoft.Practices.Unity.LifetimeManager;
using NamedTypeBuildKey = Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey;

namespace DragonSpark.IoC
{
	public class DragonSparkExtension : UnityContainerExtension
	{
		readonly BitFlipper initialize = new BitFlipper();
		internal IList<IDisposable> Disposables
		{
			get { return disposables ?? ( disposables = new List<IDisposable>() ); }
		}	IList<IDisposable> disposables;

		internal List<NamedTypeBuildKey> Items
		{
			get { return items ?? ( items = new List<NamedTypeBuildKey>() ); }
		}	List<NamedTypeBuildKey> items;

		internal IList<IUnityContainer> Children
		{
			get { return children ?? ( children = new List<IUnityContainer>() ); }
		}	IList<IUnityContainer> children;
		
		internal Dictionary<NamedTypeBuildKey,Type> MappedTypes
		{
			get { return mappedTypes ?? ( mappedTypes = new Dictionary<NamedTypeBuildKey, Type>() ); }
		}	Dictionary<NamedTypeBuildKey,Type> mappedTypes;

		internal Dictionary<NamedTypeBuildKey,List<Type>> Mappings
		{
			get { return mappings ?? ( mappings = new Dictionary<NamedTypeBuildKey,List<Type>>() ); }
		}	Dictionary<NamedTypeBuildKey,List<Type>> mappings;
		
		internal ILifetimeContainer LifetimeContainer
		{
			get { return Context.Lifetime; }
		}

		internal CurrentBuildKeyStrategy CurrentBuildKeyStrategy { get; set; }

		protected override void Initialize()
		{
			CurrentBuildKeyStrategy = new CurrentBuildKeyStrategy();

			Context.Policies.SetDefault<IConstructorSelectorPolicy>(new DefaultUnityConstructorSelectorPolicy());

			
			Context.Strategies.Add( CurrentBuildKeyStrategy, UnityBuildStage.Setup );
			Context.Strategies.AddNew<CurrentContextStrategy>( UnityBuildStage.Setup );

			Context.Strategies.AddNew<ApplicationConfigurationStrategy>( UnityBuildStage.PreCreation );
			Context.Strategies.AddNew<DefaultValueStrategy>( UnityBuildStage.Initialization );
			Context.Strategies.AddNew<ApplyBehaviorStrategy>( UnityBuildStage.Initialization );
			
			Context.RegisteringInstance += ContextRegisteringInstance;
			Context.Registering += ContextRegistering;
			Context.ChildContainerCreated += ContextChildContainerCreated;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose gets called when child container is disposed." )]
		void ContextChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
		{
			e.ChildContainer.NotNull( x =>
			{
				Children.Add( x );
				x.RegisterInstance( Container, new ContainerLifetimeManager() );
				x.RegisterInstance( new ChildContainerListener( this, x ) );
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
			readonly DragonSparkExtension extension;
			readonly IUnityContainer child;

			public ChildContainerListener( DragonSparkExtension extension, IUnityContainer child )
			{
				this.extension = extension;
				this.child = child;
			}

			/*[ContractInvariantMethod]
			void Invariant()
			{
				Contract.Invariant( extension != null && extension.Context != null && child != null );
			}*/

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
			Contract.Assume( Context != null );

			Context.RegisteringInstance -= ContextRegisteringInstance;
			Context.Registering -= ContextRegistering;
			Context.ChildContainerCreated -= ContextChildContainerCreated;
		}

		void ContextRegistering( object sender, RegisterEventArgs e )
		{
			var key = new NamedTypeBuildKey( e.TypeFrom, e.Name );
			if ( e.TypeTo != null )
			{
				if ( !MappedTypes.ContainsKey( key ) )
				{
					MappedTypes.Add( key, e.TypeTo );
				}
				RegisterMapping( new NamedTypeBuildKey( e.TypeTo, e.Name ), e.TypeFrom );
			}
			Register( key );
		}

		List<Type> ResolveMappingTypes( NamedTypeBuildKey key )
		{
			List<Type> result;
			if ( !Mappings.TryGetValue( key, out result ) )
			{
				Mappings.Add( key, result = new List<Type>() );
				Register( key );
			}
			Contract.Assume( result != null );
			return result;
		}

		void RegisterMapping( NamedTypeBuildKey key, Type type )
		{
			var types = ResolveMappingTypes( key );
			if ( !types.Contains( type ) )
			{
				types.Add( type );
			}
		}

		void Register( NamedTypeBuildKey key )
		{
			if ( !Items.Contains( key ) )
			{
				Items.Add( key );
			}
		}

		void ContextRegisteringInstance(object sender, RegisterInstanceEventArgs e)
		{
			Register( new NamedTypeBuildKey( e.RegisteredType, e.Name ) );
		}

		IUnityContainer Creator
		{
			get
			{
				Contract.Assume( Context != null && Context.Container != null );
				return creator ?? ( creator = Context.Container.CreateChildContainer() );
			}
		}	IUnityContainer creator;
		internal object Create( Type type, object[] parameters )
		{
			var values = parameters.NotNull().Select( x => 
			{
				var parameterValue = x.As<TypedInjectionValue>();
				if ( parameterValue != null )
				{
					var instance = parameterValue.GetResolverPolicy( null ).Resolve( null );
					Creator.RegisterInstance( parameterValue.ParameterType, instance );
					return instance;
				}
				x.GetType().GetInterfaces().Union( x.GetType().GetHierarchy( false ) ).Distinct().Apply( y => Creator.RegisterInstance( y, x ) );
				return x;
			} ).ToArray();

			var result = Creator.Resolve( type );
			var lifetime = Creator.GetLifetimeContainer();
			var entries = Creator.GetLifetimeEntries().Where( x => values.Contains( x.Value ) ).ToArray();
			entries.Apply( x => lifetime.Remove( x.Key ) );
			return result;
		}

		internal TResult Create<TResult>( object[] parameters )
		{
			var result = Create( typeof(TResult), parameters ).To<TResult>();
			return result;
		}

		public void Complete()
		{
			initialize.Check( () => Context.Container.AddExtension( ConfigurationExtension.Extension ) ); // HACK: Weakkkkkk SAUUUUUUUUUCEEEEEEEE!!!
		}
	}
}
