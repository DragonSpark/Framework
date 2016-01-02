using System;
using System.ComponentModel;
using System.Windows;

namespace DragonSpark.Windows.Markup
{
	public class StaticResourceExtension : System.Windows.StaticResourceExtension
	{
		public StaticResourceExtension()
		{}

		public StaticResourceExtension( object resourceKey ) : base( resourceKey )
		{}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			// ... 
			return DesignerProperties.GetIsInDesignMode( new DependencyObject() ) ? null : base.ProvideValue( serviceProvider );
		}
	}
}