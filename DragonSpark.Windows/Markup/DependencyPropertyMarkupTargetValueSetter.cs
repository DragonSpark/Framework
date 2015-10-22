using System;
using System.Windows;
using System.Windows.Threading;

namespace DragonSpark.Windows.Markup
{
	public abstract class MarkupTargetValueSetterBase : IMarkupTargetValueSetter
	{
		readonly ConditionMonitor monitor = new ConditionMonitor();
		
		public void SetValue( object value )
		{
			if ( monitor.IsApplied )
			{
				throw new ObjectDisposedException( GetType().FullName );
			}

			Apply( value );
		}

		protected abstract void Apply( object value );

		protected bool IsDisposed => monitor.IsApplied;

		public void Dispose()
		{
			monitor.Apply( () =>
			{
				Dispose( true );
				GC.SuppressFinalize( this );
			} );
		}

		protected virtual void Dispose( bool disposing )
		{}
	}

	

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
