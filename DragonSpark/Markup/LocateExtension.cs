using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Application.Markup
{
	public class MarkupExtensionMonitor
	{
		public static MarkupExtensionMonitor Instance
		{
			get { return InstanceField; }
		}	static readonly MarkupExtensionMonitor InstanceField = new MarkupExtensionMonitor();

		readonly ConditionMonitor monitor = new ConditionMonitor();
		
		public event EventHandler Initialized = delegate { };

		MarkupExtensionMonitor()
		{}

		public bool IsInitialized
		{
			get { return monitor.State != ConditionMonitorState.None; }
		}

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
		// readonly object placeholder = null;

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

		/*public override object ProvideValue( IServiceProvider serviceProvider )
		{
			this.BuildUpOnce();
			var result = IsAvailable() ? GetValue( serviceProvider ) : GetPlaceholder( serviceProvider );
			return result;
		}

		protected virtual bool IsAvailable()
		{
			var result = EventAggregator.Transform( aggregator => aggregator.GetEvent<SetupEvent>().History.Last() >= SetupStatus );
			return result;
		}

		protected abstract object GetValue( IServiceProvider serviceProvider );
		
		object GetPlaceholder( IServiceProvider serviceProvider )
		{
			serviceProvider.Get<IProvideValueTarget>().With( target => Subscribe( target.TargetProperty, target.TargetObject, () => GetValue( serviceProvider ) ) );
			return placeholder;
		}

		void Subscribe( object property, object target, Func<object> factory )
		{
			var item = property.As<PropertyInfo>() ?? property.AsTo<DependencyProperty, PropertyInfo>( x => target.GetType().GetProperty( x.Name ) );
			item.With( info =>
			{
				Defer( () => Assign( info, target, factory ) );
			} );

			property.Null( () => target.As<IList>( list => Defer( () => AssignList( list, factory ) ) ) );
		}

		protected virtual void Defer( Action action )
		{
			EventAggregator.ExecuteWhenStatusIs( SetupStatus, action );
		}

		[Default( SetupStatus.Configured )]
		public SetupStatus SetupStatus { get; set; }

		[Activate]
		IEventAggregator EventAggregator { get; set; }

		protected virtual void Assign( PropertyInfo info, object target, Func<object> factory )
		{
			factory().With( value => info.SetValue( target, value, null ) );
		}

		protected virtual void AssignList( IList list, Func<object> factory )
		{
			factory().With( value =>
			{
				list.Remove( placeholder );
				 list.Add( value );
			} );
		}*/
	}

	/*public class ActivateExtension : LocateExtension
	{
		public ActivateExtension()
		{}

		public ActivateExtension( Type type ) : base( type )
		{}

		public ActivateExtension( Type type, string buildName ) : base( type, buildName )
		{}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			return base.GetValue( serviceProvider );
		}
	}*/

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

		public Collection<PropertySetter> Properties
		{
			get { return properties; }
		}	readonly Collection<PropertySetter> properties = new Collection<PropertySetter>();
	}

	public class FactoryExtension : LocateExtension
	{
		public FactoryExtension()
		{}

		public FactoryExtension( Type type ) : base( type) 
		{}

		public FactoryExtension( Type type, object parameter ) : base( type )
		{
			Parameter = parameter;
		}

		public FactoryExtension( Type type, object parameter, string buildName ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var instance = Instance ?? base.GetValue( serviceProvider );
			var factory = instance.AsTo<IFactory,object>( x => x.Create( Type, Parameter ) );
			return factory;
		}

		public IFactory Instance { get; set; }

		public object Parameter { get; set; }
	}
}