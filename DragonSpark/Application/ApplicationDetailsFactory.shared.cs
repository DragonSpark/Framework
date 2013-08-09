using System;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application
{
	public class ApplicationDetailsFactory : FactoryBase
	{
		public Type SourceType {  get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var targetType = SourceType ?? container.TryResolve<IApplicationContext>().Transform( x => x.GetType() );
			var result = targetType.Transform( x => new ApplicationDetailsProvider( x ).RetrieveApplicationDetails() );
			return result;
		}
	}
}