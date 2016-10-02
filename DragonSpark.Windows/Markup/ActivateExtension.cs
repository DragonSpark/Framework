using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public class ActivateExtension : MarkupExtension
	{
		readonly Type type;

		public ActivateExtension( Type type )
		{
			this.type = type;
		}

		public override object ProvideValue( IServiceProvider serviceProvider ) => Activator.CreateInstance( type );
	}
}