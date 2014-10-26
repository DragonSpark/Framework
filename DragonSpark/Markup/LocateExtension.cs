using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Markup
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
}