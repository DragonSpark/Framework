using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragonSpark.Application.Client.Controls
{
	public abstract class ApplicationBarButtonBase : Button
	{}

	/// <summary>
	/// Defines an application bar icon
	/// </summary>
	public class ApplicationBarIconButton : ApplicationBarButtonBase
	{
		static ApplicationBarIconButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof(ApplicationBarIconButton), new FrameworkPropertyMetadata( typeof(ApplicationBarIconButton) ) );
		}

		[Category("Behavior"), DefaultValue(""), Description("Image source of the application bar icon"), NotifyParentProperty(true)]
		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue( ImageSourceProperty, value ); }
		}	public static DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ApplicationBarIconButton));
	}
}
