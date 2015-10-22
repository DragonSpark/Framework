using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class AssemblyVersionExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IRootObjectProvider>().Transform( x => x.RootObject.GetType().Assembly.GetName().Version );
			return result;
		}
	}

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