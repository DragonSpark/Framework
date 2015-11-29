using System.Windows;
using System.Windows.Threading;

namespace DragonSpark.Windows.Markup
{
	public sealed class DependencyPropertyMarkupTargetValueSetter : MarkupTargetValueSetterBase
	{
		readonly DependencyObject targetObject;
		readonly DependencyProperty targetProperty;

		public DependencyPropertyMarkupTargetValueSetter( DependencyObject targetObject, DependencyProperty targetProperty )
		{
			this.targetObject = targetObject;
			this.targetProperty = targetProperty;
		}

		protected override void Apply( object value )
		{
			// Marshal the call back to the UI thread
			targetObject.Dispatcher.Invoke( DispatcherPriority.Normal, (DispatcherOperationCallback)( o =>
			{
				targetObject.SetValue( targetProperty, o );
				return null;
			} ), value );
		}
	}
}
