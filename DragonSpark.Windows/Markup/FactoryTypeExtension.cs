using System;
using DragonSpark.Sources.Delegates;

namespace DragonSpark.Windows.Markup
{
	public class FactoryTypeExtension : MarkupExtensionBase
	{
		public FactoryTypeExtension( Type factoryType )
		{
			FactoryType = factoryType;
		}

		public Type FactoryType { get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) => SourceFactory.Default.Get( FactoryType );
	}
}