using System;

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
}