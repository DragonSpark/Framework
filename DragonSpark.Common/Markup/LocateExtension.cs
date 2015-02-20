using DragonSpark.Activation;
using DragonSpark.Common.Application;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Common.Markup
{
	public class SetupExtension : LocateExtension
	{
		public SetupExtension()
		{}

		public SetupExtension( Type type ) : base( type )
		{}

		public SetupExtension( Type type, string buildName ) : base( type, buildName )
		{}

		[Default( SetupStatus.Initialized )]
		public SetupStatus SetupStatus { get; set; }

		[Activate]
		IEventAggregator EventAggregator { get; set; }


		protected override void Assign( PropertyInfo info, object target, Func<object> factory )
		{
			this.BuildUpOnce();
			
			EventAggregator.ExecuteWhenStatusIs( SetupStatus, () => base.Assign( info, target, factory ) );
		}
	}

	public abstract class MarkupExtension : System.Windows.Markup.MarkupExtension
	{
		readonly object placeholder = null;


		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = ServiceLocation.IsAvailable() ? GetValue( serviceProvider ) : GetPlaceholder( serviceProvider );
			return result;
		}

		protected abstract object GetValue( IServiceProvider serviceProvider );


		object GetPlaceholder( IServiceProvider serviceProvider )
		{
			serviceProvider.Get<IProvideValueTarget>().With( target => Subscribe( target, () => GetValue( serviceProvider ) ) );
			return placeholder;
		}

		void Subscribe( IProvideValueTarget service, Func<object> factory )
		{
			var property = service.TargetProperty.As<PropertyInfo>() ?? service.TargetProperty.AsTo<DependencyProperty, PropertyInfo>( x => service.TargetObject.GetType().GetProperty( x.Name ) );
			property.With( info =>
			{
				var target = service.TargetObject;
				ServiceLocation.Assigned += ( sender, args ) => Assign( info, target, factory );
			} );

			property.Null( () => service.TargetObject.As<IList>( list => ServiceLocation.Assigned += ( sender, args ) => AssignList( list, factory ) ) );
		}

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
		}
	}

	[ContentProperty( "Properties" )]
	public class LocateExtension : MarkupExtension
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
			 var result = Type.Transform( x => ServiceLocator.Current.GetInstance( x, BuildName ) );
			result.As<ISupportInitialize>( x => x.BeginInit() );
			result.NotNull( x => Properties.Apply( y => x.GetType().GetProperty( y.PropertyName ).NotNull( z => y.Apply( z, x ) ) ) );
			result.As<ISupportInitialize>( x => x.EndInit() );
			return result;
		}

		public Collection<DragonSpark.Common.Markup.PropertySetter> Properties
		{
			get { return properties; }
		}	readonly Collection<PropertySetter> properties = new Collection<PropertySetter>();
	}

	public class FactoryExtension : LocateExtension
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryExtension"/> class.
		/// </summary>
		public FactoryExtension()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryExtension"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public FactoryExtension( Type type ) : base( type) 
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryExtension"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="parameter">The parameter.</param>
		public FactoryExtension( Type type, object parameter ) : base( type )
		{
			Parameter = parameter;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryExtension"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="parameter">The parameter.</param>
		/// <param name="buildName">Name of the build.</param>
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

		/// <summary>
		/// The instance of the factory.
		/// </summary>
		public IFactory Instance { get; set; }

		/// <summary>
		/// Gets or sets the parameter.
		/// </summary>
		/// <value>The parameter.</value>
		public object Parameter { get; set; }
	}
}