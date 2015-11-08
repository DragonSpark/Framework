using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Entity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using System.Xaml;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Markup
{
	public class MarkupExtensionMonitor
	{
		public static MarkupExtensionMonitor Instance { get; } = new MarkupExtensionMonitor();

		readonly ConditionMonitor monitor = new ConditionMonitor();
		
		public event EventHandler Initialized = delegate { };

		MarkupExtensionMonitor()
		{}

		public bool IsInitialized => monitor.State != ConditionMonitorState.None;

		public void Initialize()
		{
			monitor.Apply( () =>
			{
				Initialized( this, EventArgs.Empty );
				Initialized = delegate { };
			} );
		}
	}

	public abstract class MonitoredMarkupExtension : DeferredMarkupExtension
	{
		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			var result = MarkupExtensionMonitor.Instance.IsInitialized ? GetValue( serviceProvider ) : Listen( serviceProvider, setter );
			return result;
		}

		protected virtual object Listen( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			MarkupExtensionMonitor.Instance.Initialized += ( sender, args ) =>
			{
				var value = GetValue( serviceProvider );
				setter.SetValue( value );
			};
			return null;
		}

		protected abstract object GetValue( IServiceProvider serviceProvider );
	}

	[ContentProperty( "Properties" )]
	public class LocateExtension : MonitoredMarkupExtension
	{
		public LocateExtension()
		{}

		public LocateExtension( Type type ) : this( type, null )
		{}

		public LocateExtension( Type type, string buildName )
		{
			Type = type;
			BuildName = buildName;
		}

		public Type Type { get; set; }

		public string BuildName { get; set; }

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			 var result = Type.Transform( x => Activator.CreateNamedInstance<object>( x, BuildName ) );
			result.As<ISupportInitialize>( x => x.BeginInit() );
			result.NotNull( x => Properties.Apply( y => x.GetType().GetProperty( y.PropertyName ).NotNull( z => y.Apply( z, x ) ) ) );
			result.As<ISupportInitialize>( x => x.EndInit() );
			return result;
		}

		public Collection<PropertySetter> Properties { get; } = new Collection<PropertySetter>();
	}

	[MarkupExtensionReturnType( typeof(InjectionMember) )]
	public class InjectionFactoryExtension : FactoryExtension
	{
		public InjectionFactoryExtension() : base( typeof(InjectionMember) )
		{}

		protected override IFactory DetermineFactory( IServiceProvider serviceProvider )
		{
			return new InjectionFactoryFactory( Instance, Parameter );
		}

		protected override object DetermineParameter( IServiceProvider serviceProvider )
		{
			var targetType = serviceProvider.Get<DeferredContext>().RegistrationType;
			var result = new InjectionMemberContext( Services.Locate<IUnityContainer>(), targetType );
			return result;
		}

		static Type Resolve( IServiceProvider serviceProvider )
		{
			var xamlType = serviceProvider.Get<IXamlSchemaContextProvider>().SchemaContext.GetXamlType( typeof(UnityRegistrationCommand) );
			var member = xamlType.GetMember( "RegistrationType" );
			var value = serviceProvider.Get<IAmbientProvider>().GetFirstAmbientValue( new [] { xamlType }, member );
			var result = (Type)value.Value;

			return result;
		}

		protected override IServiceProvider Prepare( IServiceProvider serviceProvider, IProvideValueTarget target, IMarkupTargetValueSetterBuilder builder )
		{
			var result = new DeferredContext( serviceProvider, target.TargetObject, target.TargetProperty, builder.GetPropertyType( target ), Resolve( serviceProvider ) );
			return result;
		}

		public class DeferredContext : Markup.DeferredContext
		{
			public DeferredContext( IServiceProvider inner, object targetObject, object targetProperty, Type propertyType, Type registrationType ) : base( inner, targetObject, targetProperty, propertyType )
			{
				RegistrationType = registrationType;
			}

			public Type RegistrationType { get; }
		}
	}

	public class FactoryExtension : LocateExtension
	{
		public FactoryExtension()
		{}

		public FactoryExtension( Type type ) : this( type, null ) 
		{}

		public FactoryExtension( Type type, object parameter ) : this( type, null, parameter )
		{}

		public FactoryExtension( Type type, string buildName, object parameter ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = DetermineFactory( serviceProvider ).Transform( x => Create( x, serviceProvider ) );
			return result;
		}

		protected virtual object Create( IFactory factory, IServiceProvider serviceProvider )
		{
			var type = serviceProvider.Get<DeferredContext>().PropertyType;
			var result = factory.Create( type, DetermineParameter( serviceProvider ) );
			return result;
		}

		protected virtual IFactory DetermineFactory( IServiceProvider serviceProvider )
		{
			var result = Instance ?? base.GetValue( serviceProvider ) as IFactory;
			return result;
		}

		protected virtual object DetermineParameter( IServiceProvider serviceProvider )
		{
			return Parameter;
		}

		public IFactory Instance { get; set; }

		public object Parameter { get; set; }
	}

	public interface IIncrementer
	{
		int Next( object context );
	}

	class Incrementer : IIncrementer
	{
		readonly IDictionary<WeakReference, int>  items = new Dictionary<WeakReference, int>();

		public int Next( object context )
		{
			var key = Items.Keys.SingleOrDefault( reference => reference.Target == context ) ?? new WeakReference( context );
			var current = items.Ensure( key, reference => 0 ) + 1;
			var result = items[key] = current;
			return result;
		}

		IDictionary<WeakReference, int> Items
		{
			get
			{
				items.Keys.Where( reference => !reference.IsAlive ).ToArray().Apply( reference => items.Remove( reference ) );
				return items;
			}
		}
	}

	[MarkupExtensionReturnType( typeof(int) )]
	public class NextExtension : MarkupExtension
	{
		public NextExtension()
		{
			this.WithDefaults();
		}

		[Activate]
		public IIncrementer Incrementer { get; set; }

		[Activate]
		public AllTypesOfFactory Factory { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var context = DetermineContext( serviceProvider );
			var result = Incrementer.Next( context );
			return result;
		}

		protected virtual object DetermineContext( IServiceProvider serviceProvider )
		{
			var xamlSchemaContext = serviceProvider.Get<IXamlSchemaContextProvider>().SchemaContext;
			var types = new[] { typeof(DragonSpark.Runtime.Collection) }; // TODO: More general/generic implementation.
			var context = serviceProvider.Get<IAmbientProvider>().GetFirstAmbientValue( types.Select( xamlSchemaContext.GetXamlType ).ToArray() );
			return context;
		}
	}
}