using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Markup
{
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

		class ProvisionContext
		{
			readonly LocateExtension owner;

			readonly object replacement = null;

			public ProvisionContext( LocateExtension owner, IServiceProvider serviceProvider )
			{
				this.owner = owner;
				Type = owner.Type;
				ServiceLocation.IsAvailable().IsFalse( () => serviceProvider.Get<IProvideValueTarget>().With( Subscribe ) );
			}

			void Subscribe( IProvideValueTarget obj )
			{
				var property = obj.TargetProperty.As<PropertyInfo>( x =>
				{
					var target = obj.TargetObject;
					ServiceLocation.Assigned += ( sender, args ) => ProvideValue().NotNull( y => x.SetValue( target, y, null ) );
				} );

				property.Null( () => obj.TargetObject.As<IList>( x => ServiceLocation.Assigned += ( sender, args ) => ProvideValue().NotNull( y =>
				{
					x.Remove( replacement );
					x.Add( y );
				} ) ) );
			}

			public object ProvideValue()
			{
				var result = ServiceLocation.IsAvailable() ? owner.Create( Type ) : replacement;
				return result;
			}

			Type Type { get; set; }
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new ProvisionContext( this, serviceProvider ).ProvideValue();
			return result;
		}

		protected virtual object Create( Type type )
		{
			var result = type.Transform( x => ServiceLocator.Current.GetInstance( x, BuildName ) );
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

		/// <summary>
		/// Creates the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		protected override object Create( Type type )
		{
			var instance = Instance ?? base.Create( Type );
			var factory = instance.AsTo<IFactory,object>( x => x.Create( type, Parameter ) );
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