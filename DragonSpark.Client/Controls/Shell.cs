using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using DragonSpark.Extensions;

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

		/*public DataTemplate TitleLinkTemplate
		{
			get { return GetValue( TitleLinkTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( TitleLinkTemplateProperty, value ); }
		}	public static readonly DependencyProperty TitleLinkTemplateProperty = DependencyProperty.Register( "TitleTitleLinkTemplate", typeof(DataTemplate), typeof(Shell), null );

		public IEnumerable TitleLinksSource
		{
			get { return GetValue( TitleLinksSourceProperty ).To<IEnumerable>(); }
			set { SetValue( TitleLinksSourceProperty, value ); }
		}	public static readonly DependencyProperty TitleLinksSourceProperty = DependencyProperty.Register( "TitleLinksSource", typeof(IEnumerable), typeof(Shell), new PropertyMetadata( OnItemsChanged ) );

		static void OnItemsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<Shell>( shell =>
			{
				shell.TitleLinks.Clear();
				var links = e.NewValue.To<IEnumerable>().Cast<object>().Select( item => shell.TitleLinkTemplate.LoadContent().AsTo<ContentControl, Link>( control => control.Content.To<Link>() ) );
				shell.TitleLinks.AddRange( links );

			} );
		}*/

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

		/*public ImageSource LogoSource
		{
			get { return (ImageSource)GetValue(LogoSourceProperty); }
			set { SetValue( LogoSourceProperty, value ); }
		}	public static DependencyProperty LogoSourceProperty = DependencyProperty.Register("LogoSource", typeof(ImageSource), typeof(Shell));*/
	}
}
