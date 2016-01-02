using System;
using DragonSpark.Extensions;

namespace DragonSpark.Markup
{
	public class IsReleaseExtension : IsDebugExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = !base.ProvideValue( serviceProvider ).To<bool>();
			return result;
		}
	}
}