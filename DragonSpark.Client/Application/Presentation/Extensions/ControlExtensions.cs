using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class ControlExtensions
	{
		public static bool FocusNext( this FrameworkElement target )
		{
			var controls = target.GetAllChildren<Control>().Where( x => x.GetType() != typeof(ContentControl) ).ToList();
			var focused = FocusManager.GetFocusedElement().As<Control>();
			var result = controls.Remove( focused ) && controls.Any( x =>
			{
			    var focus = x.Focus();
			    return focus;
			} );
			focused.Focus();
			return result;
		}
	}
}
