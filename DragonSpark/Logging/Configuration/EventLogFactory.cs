using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;

namespace DragonSpark.Logging.Configuration
{
	public class EventLogFactory : FactoryBase
	{
		[DefaultPropertyValue( "" )]
		public string Name { get; set; }

		[DefaultPropertyValue( "" )]
		public string Source { get; set; }
		
		[DefaultPropertyValue( "." )]
		public string MachineName { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = new EventLog( Name, MachineName, Source );
			return result;
		}
	}
}