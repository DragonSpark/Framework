using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class InjectionFactoryInstance : FactoryBase
	{
		public IFactory Instance { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = Instance.Transform( x => x.Create( type, container ) );
			return result;
		}
	}
}