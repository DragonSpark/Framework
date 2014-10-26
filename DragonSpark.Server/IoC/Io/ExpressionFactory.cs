using System;
using System.Windows.Markup;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects.Synchronization;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Io
{
	[ContentProperty( "Source" )]
	public class ExpressionFactory : FactoryBase
	{
		public NamedTypeBuildKey Source { get; set; }

		public string Expression { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var source = container.Resolve( Source.BuildType, Source.BuildName );
			var result = source.EvaluateValue( Expression );
			return result;
		}
	}
}


