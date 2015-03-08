using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class Type : MarkupExtension
	{
		public string TypeName { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var type = DetermineType( serviceProvider, TypeName );
			var result = type.Transform( x => ResolveValue( serviceProvider, x ) );
			return result;
		}

		protected System.Type DetermineType( IServiceProvider serviceProvider, string typeName )
		{
			var result = serviceProvider.GetService( typeof(IXamlTypeResolver) ).AsTo<IXamlTypeResolver, System.Type>( x => ResolveType( x, typeName ) );
			return result;
		}

		protected virtual System.Type ResolveType( IXamlTypeResolver xamlTypeResolver, string typeName )
		{
			var result = xamlTypeResolver.ResolveType( typeName );
			return result;
		}

		protected virtual object ResolveValue( IServiceProvider serviceProvider, System.Type type )
		{
			return type;
		}
	}

	[ContentProperty( "Properties" )]
	public class LocateExtension : MarkupExtension
	{
		public LocateExtension()
		{}

		public LocateExtension( System.Type type ) : this( type, null )
		{}

		public LocateExtension( System.Type type, string buildName )
		{
			Type = type;
			BuildName = buildName;
		}

		public LocateExtension( string typeName ) : this( typeName, null )
		{}

		public LocateExtension( string typeName, string buildName )
		{
			TypeName = typeName;
			BuildName = buildName;
		}

		public System.Type Type { get; set; }

		public string TypeName { get; set; }

		public string BuildName { get; set; }

		class ProvisionContext
		{
			readonly LocateExtension owner;

			public ProvisionContext( LocateExtension owner, IServiceProvider serviceProvider )
			{
				this.owner = owner;
				Type = owner.Type ?? serviceProvider.Get<IXamlTypeResolver>().ResolveType( owner.TypeName );
				ServiceLocation.IsAvailable().IsFalse( () => serviceProvider.GetService( typeof(IProvideValueTarget) ).As<IProvideValueTarget>( x => x.TargetProperty.As<PropertyInfo>( y =>
				{
					var target = x.TargetObject;
					ServiceLocation.Assigned += ( sender, args ) => ProvideValue().NotNull( z => y.SetValue( target, z, null ) );
				} ) ) );
			}

			public object ProvideValue()
			{
				var result = ServiceLocation.IsAvailable() ? ResolveValue() : null;
				return result;
			}

			object ResolveValue()
			{
				var result = Type.Transform( x => ServiceLocator.Current.GetInstance( x, owner.BuildName ) );
				result.As<ISupportInitialize>( x => x.BeginInit() );
				result.NotNull( x => owner.Properties.Apply( y => x.GetType().GetProperty( y.PropertyName ).NotNull( z => y.Apply( z, x ) ) ) );
				result.As<ISupportInitialize>( x => x.EndInit() );
				return result;
			}

			System.Type Type { get; set; }
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new ProvisionContext( this, serviceProvider ).ProvideValue();
			return result;
		}

		public Collection<PropertySetter> Properties
		{
			get { return properties; }
		}	readonly Collection<PropertySetter> properties = new Collection<PropertySetter>();
	}
}