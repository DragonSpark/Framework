using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	[MarkupExtensionReturnType( typeof(string) )]
	public class NameExtension : MarkupExtension
	{
		public NameExtension( Type subject = null )
		{
			Subject = subject;
		}

		public Type Subject { get; set; }

		public override object ProvideValue( IServiceProvider serviceProvider ) => Subject?.Name;
	}
}