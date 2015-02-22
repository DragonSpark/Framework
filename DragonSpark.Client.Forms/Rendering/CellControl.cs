using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.Rendering
{
	public class CellControl : ContentControl
	{
		public static readonly DependencyProperty CellProperty = DependencyProperty.Register("Cell", typeof(object), typeof(CellControl), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((CellControl)o).SetSource((Cell)e.OldValue, (Cell)e.NewValue);
		}));
		private readonly PropertyChangedEventHandler propertyChangedHandler;
		public Cell Cell
		{
			get
			{
				return (Cell)base.GetValue(CellControl.CellProperty);
			}
			set
			{
				base.SetValue(CellControl.CellProperty, value);
			}
		}
		public CellControl()
		{
			base.Unloaded += delegate(object sender, RoutedEventArgs args)
			{
				Cell cell = base.DataContext as Cell;
				if (cell != null)
				{
					cell.SendDisappearing();
				}
			};
			this.propertyChangedHandler = new PropertyChangedEventHandler(this.OnCellPropertyChanged);
		}
		private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HasContextActions")
			{
				this.SetupContextMenu();
			}
		}
		private System.Windows.DataTemplate GetTemplate(Cell cell)
		{
			ICellRenderer handler = Registrar.Registered.GetHandler<ICellRenderer>(cell.GetType());
			return handler.GetTemplate(cell);
		}
		private void SetSource(Cell oldCell, Cell newCell)
		{
			if (oldCell != null)
			{
				oldCell.PropertyChanged -= this.propertyChangedHandler;
				oldCell.SendDisappearing();
			}
			if (newCell != null)
			{
				newCell.SendAppearing();
				System.Windows.DataTemplate template = this.GetTemplate(newCell);
				base.Content = template.LoadContent();
				this.SetupContextMenu();
				newCell.PropertyChanged += this.propertyChangedHandler;
				return;
			}
			base.Content = null;
		}
		private void SetupContextMenu()
		{
			if (base.Content == null)
			{
				return;
			}
			if (!this.Cell.HasContextActions)
			{
				ContextMenuService.SetContextMenu((DependencyObject)base.Content, null);
				return;
			}
			ContextMenu contextMenu = new CustomContextMenu();
			contextMenu.SetBinding(ItemsControl.ItemsSourceProperty, new System.Windows.Data.Binding("ContextActions"));
			ContextMenuService.SetContextMenu((DependencyObject)base.Content, contextMenu);
		}
	}
}
