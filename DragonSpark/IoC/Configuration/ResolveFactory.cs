using DragonSpark.Objects;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics.Contracts;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Item" )]
	public class Instance : FactoryBase
	{
		public object Item { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			return Item;
		}
	}

	[MarkupExtensionReturnType( typeof(LocationFactory) )]
	public class LocationFactoryExtension : MarkupExtension
	{
		readonly Type locationType;
		readonly string name;

		public LocationFactoryExtension( Type locationType ) : this( locationType, null )
		{}

		public LocationFactoryExtension( Type locationType, string name )
		{
			this.locationType = locationType;
			this.name = name;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new LocationFactory { LocationType = locationType, BuildName = name };
			return result;
		}
	}

	public class LocationFactory : FactoryBase
	{
		public Type LocationType { get; set; }

		public string BuildName { get; set; }

		protected override object Create(IUnityContainer container, Type type, string buildName)
		{
			var result = container.Resolve( LocationType ?? type, BuildName ?? buildName );
			return result;
		}
	}

	public class ResolveFactory : FactoryBase
	{
		public string FactoryBuildName { get; set; }

		protected override object Create(IUnityContainer container, Type type, string buildName)
		{
			Contract.Assume( container != null );
			Contract.Assume( type != null );

			var factory = container.Resolve<IFactory>( FactoryBuildName );
			var result = factory.Create( type, container );
			return result;
		}
	}
}