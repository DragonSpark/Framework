using System.ComponentModel;
using System.Windows;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Controls
{
	public partial class DialogChrome 
#if SILVERLIGHT
		: ChildWindow, System.Windows.Controls.INavigate
#else
		: Window
#endif
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
		public static DependencyProperty ButtonsProperty =
			DependencyProperty.Register(
				"Buttons",
				typeof(IObservableCollection<ButtonModel>),
				typeof(DialogChrome),
				null
				);

		public DialogChrome()
		{
			DefaultStyleKey = typeof(DialogChrome);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Intended to be set via Xaml." ), TypeConverter(typeof(ButtonConverter))]
		public IObservableCollection<ButtonModel> Buttons
		{
			get { return GetValue(ButtonsProperty) as IObservableCollection<ButtonModel>; }
			set { SetValue(ButtonsProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			var chrome = GetTemplateChild("Chrome") as UIElement;
			if (chrome != null && Title == null) chrome.Visibility = Visibility.Collapsed;
		}

		protected override void OnClosed( System.EventArgs e )
		{
			Visibility = Visibility.Collapsed;
			base.OnClosed( e );
		}
	}
}