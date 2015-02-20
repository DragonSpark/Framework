using DragonSpark.Extensions;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DragonSpark.Client.Windows.Controls
{
	public class Shell : ModernWindow
	{
		static Shell()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof(Shell), new FrameworkPropertyMetadata( typeof(Shell) ) );
		}

		public DataTemplate TitleLinkTemplate
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
		}
	}
}
