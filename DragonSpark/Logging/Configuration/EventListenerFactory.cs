using System;
using System.Diagnostics.Tracing;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.Unity;

namespace DragonSpark.Logging.Configuration
{
	public abstract class EventListenerFactory : FactoryBase
	{
		public NamedTypeBuildKey Formatter { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var formatter = Formatter.Transform( x => x.Create<IEventTextFormatter>( container ), () => new JsonEventTextFormatter( EventTextFormatting.Indented ) );

			var result = CreateListener( container, formatter );
			return result;
		}

		protected abstract EventListener CreateListener( IUnityContainer container, IEventTextFormatter formatter );
	}
}