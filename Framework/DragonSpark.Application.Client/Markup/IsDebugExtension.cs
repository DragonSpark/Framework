using System;
using System.Diagnostics;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Markup
{
	public class IsDebugExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetAttribute<DebuggableAttribute>() != null;
			return result;
		}
	}
}