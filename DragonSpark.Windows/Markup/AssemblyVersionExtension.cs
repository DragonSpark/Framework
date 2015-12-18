using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class AssemblyVersionExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().With( x => x.RootObject.GetType().Assembly.GetName().Version );
			return result;
		}
	}
}