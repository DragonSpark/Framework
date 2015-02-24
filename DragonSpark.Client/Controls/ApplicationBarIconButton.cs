using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DragonSpark.Application.Client.Controls
{
	/// <summary>
	/// Defines an application bar icon
	/// </summary>
	public class ApplicationBarIconButton : FrameworkElement, INotifyPropertyChanged
	{
		// Property Changed Event
		public event PropertyChangedEventHandler PropertyChanged;

		#region DependencyProperties

		/// <summary>
		/// Command DependencyProeprty
		/// </summary>
		public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof (ICommand), typeof (ApplicationBarIconButton));

		/// <summary>
		/// Description Dependency Property
		/// </summary>
		public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ApplicationBarIconButton));

		/// <summary>
		/// Image Source Dependency Property
		/// </summary>
		public static DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ApplicationBarIconButton));

		/// <summary>
		/// IsDefault Dependency Property
		/// </summary>
		public static DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof (bool), typeof (ApplicationBarIconButton));

		/// <summary>
		/// IsCancel Dependency Property
		/// </summary>
		public static DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof (bool), typeof (ApplicationBarIconButton));

		#endregion

		/// <summary>
		/// Gets or sets the image source 
		/// </summary>
		[Category("Behavior"), DefaultValue(""), Description("Image source of the application bar icon"), NotifyParentProperty(true)]
		public ImageSource ImageSource
		{
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set
			{
				if ( !Equals( ImageSource, value ) )
				{
					SetValue( ImageSourceProperty, value );
					OnNotifyPropertyChanged( "ImageSource" );
				}
			}
		}

		/// <summary>
		/// Gets or sets a flag that defines if the command is used as the default command
		/// </summary>
		[Category("Behavior"), DefaultValue(false), Description("Defines the default command"), NotifyParentProperty(true)]
		public bool IsDefault
		{
			get { return (bool) GetValue(IsDefaultProperty); }
			set
			{
				SetValue(IsDefaultProperty, value);
				OnNotifyPropertyChanged("IsDefault");
			}
		}

		/// <summary>
		/// Gets or sets a flag that defines if the command is used as the Cancel command
		/// </summary>
		[Category("Behavior"), DefaultValue(false), Description("Defines the Cancel command"), NotifyParentProperty(true)]
		public bool IsCancel
		{
			get { return (bool) GetValue(IsCancelProperty); }
			set
			{
				SetValue(IsCancelProperty, value);
				OnNotifyPropertyChanged("IsCancel");
			}
		}

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
