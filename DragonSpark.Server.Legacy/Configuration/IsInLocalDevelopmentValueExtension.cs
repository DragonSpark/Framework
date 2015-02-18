using System;
using System.Windows.Markup;

namespace DragonSpark.Server.Legacy.Configuration
{
	public class IsInLocalDevelopmentValueExtension : MarkupExtension
	{
		readonly object localValue;
		readonly object notLocalValue;

		public IsInLocalDevelopmentValueExtension( object localValue, object notLocalValue )
		{
			this.localValue = localValue;
			this.notLocalValue = notLocalValue;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = Runtime.IsInLocalDevelopmentEnvironment() ? localValue : notLocalValue;
			return result;
		}
	}
}