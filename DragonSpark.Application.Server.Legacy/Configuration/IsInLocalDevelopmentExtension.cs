using System;
using System.Windows.Markup;

namespace DragonSpark.Server.Legacy.Configuration
{
	public class IsInLocalDevelopmentExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = Runtime.IsInLocalDevelopmentEnvironment();
			return result;
		}
	}
}