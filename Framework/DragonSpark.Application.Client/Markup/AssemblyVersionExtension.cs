using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Markup
{
	public class AssemblyVersionExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetName().Version;
			return result;
		}
	}
}