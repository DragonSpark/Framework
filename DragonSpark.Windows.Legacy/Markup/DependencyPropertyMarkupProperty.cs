using System.Windows;
using System.Windows.Threading;

namespace DragonSpark.Windows.Legacy.Markup
{
	public sealed class DependencyPropertyMarkupProperty : MarkupPropertyBase
	{
		readonly DependencyObject targetObject;
		readonly DependencyProperty targetProperty;

		public DependencyPropertyMarkupProperty( DependencyObject targetObject, DependencyProperty targetProperty ) : base( PropertyReference.New( targetProperty ) )
		{
			this.targetObject = targetObject;
			this.targetProperty = targetProperty;
		}

		protected override object OnGetValue() => targetObject.GetValue( targetProperty );

		protected override object Apply( object value )
		{
			// Marshal the call back to the UI thread
			targetObject.Dispatcher.Invoke( DispatcherPriority.Normal, (DispatcherOperationCallback)( o =>
			{
				targetObject.SetValue( targetProperty, o );
				return null;
			} ), value );
			return null;
		}
	}
}
