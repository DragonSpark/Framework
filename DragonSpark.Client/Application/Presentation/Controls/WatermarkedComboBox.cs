using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	public class WatermarkedComboBox : ComboBox
	{
		public WatermarkedComboBox()
		{
			DefaultStyleKey = typeof(WatermarkedComboBox);
		}

		public object WatermarkContent
		{
			get { return GetValue( WatermarkContentProperty ).To<object>(); }
			set { SetValue( WatermarkContentProperty, value ); }
		}	public static readonly DependencyProperty WatermarkContentProperty = DependencyProperty.Register( "WatermarkContent", typeof(object), typeof(WatermarkedComboBox), null );

		public DataTemplate WatermarkContentTemplate
		{
			get { return GetValue( WatermarkContentTemplateProperty ).To<DataTemplate>(); }
			set { SetValue( WatermarkContentTemplateProperty, value ); }
		}	public static readonly DependencyProperty WatermarkContentTemplateProperty = DependencyProperty.Register( "WatermarkContentTemplate", typeof(DataTemplate), typeof(WatermarkedComboBox), null );
	}
}