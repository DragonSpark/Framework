using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DragonSpark.Application.Client.Controls
{
	/// <summary>
	/// Defines an application bar menu item
	/// </summary>
	public class ApplicationBarMenuItem : FrameworkElement, INotifyPropertyChanged
	{
		// Property Changed Event
		public event PropertyChangedEventHandler PropertyChanged;

		#region DependencyProperties

		/// <summary>
		/// Command DependencyProeprty
		/// </summary>
		public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ApplicationBarMenuItem), null);

		/// <summary>
		/// Description Dependency Property
		/// </summary>
		public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ApplicationBarMenuItem));

		#endregion

		/// <summary>
		/// Gets or sets the application bar icon description
		/// </summary>
		[Category("Behavior"), DefaultValue(""), Description("Description of the application bar icon"), NotifyParentProperty(true)]
		public string Description
		{
			get { return (string)GetValue(DescriptionProperty); }
			set
			{
				if (Description == value)
					return;

				SetValue(DescriptionProperty, value);
				OnNotifyPropertyChanged("Description");
			}
		}

		/// <summary>
		/// Gets or sets a command that will be fired, when an applciation bar icon has been pressed
		/// </summary>
		[Category("Behavior"), Description("Command bound to the application bar icon"), NotifyParentProperty(true)]
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set
			{
				if (Command == value)
					return;

				SetValue(CommandProperty, value);
				OnNotifyPropertyChanged("Command");
			}
		}

		/// <summary>
		/// Notify on property change
		/// </summary>
		/// <param name="property">name of the property</param>
		private void OnNotifyPropertyChanged(string property)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
		}
	}
}