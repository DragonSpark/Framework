
using System.Windows;

namespace DragonSpark.Application.Client.Controls
{
	/// <summary>
	/// Defines an application bar menu item
	/// </summary>
	public class ApplicationBarMenuItem : ApplicationBarButtonBase
	{
		static ApplicationBarMenuItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata( typeof(ApplicationBarMenuItem), new FrameworkPropertyMetadata( typeof(ApplicationBarMenuItem) ) );
		}
	}
}