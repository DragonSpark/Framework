using System;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class ActivateExtension : MarkupExtension
	{
		readonly Type type;

		public ActivateExtension( Type type )
		{
			this.type = type;
		}

		public override object ProvideValue( IServiceProvider serviceProvider ) => Activator.Default.Get( type );
	}
}