using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "BuildKey" )]
	public class InjectionFactory : FactoryBase
	{
		public NamedTypeBuildKey BuildKey { get; set; }

		public object Parameter { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var factory = BuildKey.Transform( x => x.Create<IFactory>( container ) );

			var parameter = Parameter.AsTo<NamedTypeBuildKey,object>( x => x.Create( container ) ) ?? Parameter;

			var result = factory.Transform( x => x.Create( type, parameter ) );
			return result;
		}
	}
}