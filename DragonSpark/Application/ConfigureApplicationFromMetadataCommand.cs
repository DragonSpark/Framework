using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application
{
	public class ConfigureApplicationFromMetadataCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			AppDomain.CurrentDomain.GetAllTypesWith<ConfiguresApplicationAttribute>().Select( x => container.Resolve( x.Item2 ) ).OfType<IContainerConfigurationCommand>().Apply( x => x.Configure( container ) );
		}
	}
}