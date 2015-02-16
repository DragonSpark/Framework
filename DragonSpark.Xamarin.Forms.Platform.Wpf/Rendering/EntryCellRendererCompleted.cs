using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class EntryCellRendererCompleted : ICommand
	{
		public event EventHandler CanExecuteChanged = delegate {};
		public bool CanExecute(object parameter)
		{
			return true;
		}
		public void Execute(object parameter)
		{
			EntryCell entryCell = (EntryCell)parameter;
			entryCell.SendCompleted();
		}
	}
}
