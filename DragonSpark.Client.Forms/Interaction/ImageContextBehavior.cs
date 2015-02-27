
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Application.Client.Forms.Rendering;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Client.Forms.Interaction
{
	public class Host : Behavior<FrameworkElement>
	{
		public object Item
		{
			get { return GetValue( ItemProperty ).To<object>(); }
			set { SetValue( ItemProperty, value ); }
		}	public static readonly DependencyProperty ItemProperty = DependencyProperty.Register( "Item", typeof(object), typeof(Host), null );
	}
}
