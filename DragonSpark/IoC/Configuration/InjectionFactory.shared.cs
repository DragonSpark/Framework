using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "BuildKey" )]
	public class InjectionFactory : FactoryBase
	{
		public NamedTypeBuildKey BuildKey { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var factory = BuildKey.Transform( x => NamedTypeBuildKeyExtensions.Create<IFactory>( x, container ) );
			var result = factory.Transform( x => x.Create( type, container ) );
			return result;
		}
	}
}