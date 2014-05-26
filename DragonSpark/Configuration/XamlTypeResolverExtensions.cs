using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	public static class XamlTypeResolverExtensions
	{
		public static Type ResolveType( this IXamlTypeResolver target, string typeName )
		{
			try
			{
				var result = target.Resolve( typeName );
				return result;
			}
			catch ( NotSupportedException )
			{
				var assemblies = new[] {typeof(Uri).Assembly, typeof(Type).Assembly/*, typeof(IClientChannel).Assembly*/};
				var name = typeName.Split( ':' )[1];
				var result = assemblies.SelectMany( x => x.GetExportedTypes() ).FirstOrDefault( y => y.Name == name );
				return result;
			}
		}
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

		protected override object Create( Type type )
		{
			var factory = base.Create( Type ).AsTo<IFactory,object>( x => x.Create( type, Parameter ) );
			return factory;
		}
		
		public object Parameter { get; set; }
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

		class ProvisionContext
		{
			readonly LocateExtension owner;

			readonly object replacement = null;

			public ProvisionContext( LocateExtension owner, IServiceProvider serviceProvider )
			{
				this.owner = owner;
				Type = owner.Type /*?? serviceProvider.Get<IXamlTypeResolver>().ResolveType( owner.TypeName )*/;
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

	[ContentProperty( "Value" )]
	public class PropertySetter
	{
		public string PropertyName { get; set; }

		public object Value { get; set; }

		protected internal virtual void Apply( PropertyInfo info, object target )
		{
			info.SetValue( target, Value, null );
		}
	}
}