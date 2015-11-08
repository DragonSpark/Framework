using Xamarin.Forms;
using Application = System.Windows.Application;
using DataTemplate = System.Windows.DataTemplate;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering.Templates
{
	/// <summary>
	/// Interaction logic for ViewCellRenderer.xaml
	/// </summary>
	public partial class ViewCellRenderer : ICellRenderer
	{
		public ViewCellRenderer()
		{
			InitializeComponent();
		}

		public DataTemplate GetTemplate( Cell cell )
		{
			Application.Current.Resources.
			return this;
		}
	}
}
