using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
	public class ButtonPanel : ItemsControl
	{
		public readonly static DependencyProperty ButtonsProperty =
			DependencyProperty.Register(
				"Buttons",
				typeof(IObservableCollection<ButtonModel>),
				typeof(ButtonPanel),
				new PropertyMetadata(ButtonsChanged)
				);

		public ButtonPanel()
		{
			Visibility = Visibility.Collapsed;
			DefaultStyleKey = typeof(ButtonPanel);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Intended to be set via Xaml."), TypeConverter(typeof(ButtonConverter))]
		public IObservableCollection<ButtonModel> Buttons
		{
			get { return GetValue(ButtonsProperty) as IObservableCollection<ButtonModel>; }
			set { SetValue(ButtonsProperty, value); }
		}

		private static void ButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var list = e.NewValue as IObservableCollection<ButtonModel>;
			var panel = (ItemsControl)d;

			panel.Items.Clear();

			if(list != null)
				list.Apply(x => panel.Items.Add(x));

			panel.Visibility = panel.Items.Count < 1
			                   	? Visibility.Collapsed
			                   	: Visibility.Visible;
		}
	}
}