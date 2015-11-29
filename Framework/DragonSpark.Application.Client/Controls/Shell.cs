using DragonSpark.Extensions;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;

namespace DragonSpark.Application.Client.Controls
{
	public class Shell : ModernWindow
	{
		static Shell()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof(Shell), new FrameworkPropertyMetadata( typeof(Shell) ) );
		}

		public Shell()
		{
			DefaultStyleKey = typeof(Shell);
		}

		public DataTemplate LogoContentTemplate
		{
			get { return GetValue( LogoContentTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( LogoContentTemplateProperty, value ); }
		}	public static readonly DependencyProperty LogoContentTemplateProperty = DependencyProperty.Register( "LogoContentTemplate", typeof(DataTemplate), typeof(Shell), null );

		public object LogoContent
		{
			get { return GetValue( LogoContentProperty ).To<object>(); }
			set { SetValue( LogoContentProperty, value ); }
		}	public static readonly DependencyProperty LogoContentProperty = DependencyProperty.Register( "LogoContent", typeof(object), typeof(Shell), null );
	}
}
