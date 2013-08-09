using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	public partial class Menu : Expander
	{
		/*public object IsOpen
		{
			get { return GetValue( IsOpenProperty ).To<object>(); }
			set { SetValue( IsOpenProperty, value ); }
		}	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register( "IsOpen", typeof(object), typeof(Menu), new PropertyMetadata( IsOpenChanged ) );

		// HACK: For some reason, databinding does not update on boolean properties.
		static void IsOpenChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<Menu>( x => x.IsExpanded = e.NewValue.To<bool>() );
		}*/

		public object MenuContent
		{
			get { return GetValue( MenuContentProperty ).To<object>(); }
			set { SetValue( MenuContentProperty, value ); }
		}	public static readonly DependencyProperty MenuContentProperty = DependencyProperty.Register( "MenuContent", typeof(object), typeof(Menu), null );

		public DataTemplate MenuContentTemplate
		{
			get { return GetValue( MenuContentTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( MenuContentTemplateProperty, value ); }
		}	public static readonly DependencyProperty MenuContentTemplateProperty = DependencyProperty.Register( "MenuContentTemplate", typeof(DataTemplate), typeof(Menu), null );
	}
}