using System;
using System.Diagnostics.Contracts;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;
using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public class ResolveFactory : FactoryBase
	{
		//[TypeConverter( typeof(TypeNameConverter) )]
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