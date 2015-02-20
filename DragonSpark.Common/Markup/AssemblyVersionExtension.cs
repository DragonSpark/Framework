using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Common.Markup
{
	public class AssemblyVersionExtension : MarkupExtension
	{
		protected override object GetValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().RootObject.GetType().Assembly.GetName().Version;
			return result;
		}
	}
}