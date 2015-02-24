using System;
using System.Windows;
using System.Windows.Threading;

namespace DragonSpark.Application.Markup
{
	public sealed class DependencyPropertyMarkupTargetValueSetter : IMarkupTargetValueSetter
	{
		readonly ConditionMonitor monitor = new ConditionMonitor();
		readonly DependencyObject targetObject;
		readonly DependencyProperty targetProperty;

		public DependencyPropertyMarkupTargetValueSetter( DependencyObject targetObject, DependencyProperty targetProperty )
		{
			this.targetObject = targetObject;
			this.targetProperty = targetProperty;
		}

		public void SetValue( object value )
		{
			if ( targetObject == null || targetProperty == null )
			{
				throw new ObjectDisposedException( GetType().FullName );
			}

			// Marshal the call back to the UI thread
			targetObject.Dispatcher.Invoke( DispatcherPriority.Normal, (DispatcherOperationCallback)( o =>
			{
				targetObject.SetValue( targetProperty, o );
				return null;
			} ), value );
		}

		public void Dispose()
		{
			// GC.SuppressFinalize( this );
			// targetObject = null;
			// targetProperty = null;
		}
	}
}
