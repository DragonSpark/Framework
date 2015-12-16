using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public class ActivateExtension : MarkupExtension
	{
		readonly Type type;

		public ActivateExtension( Type type )
		{
			this.type = type;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = Activator.CreateInstance( type );
			return result;
		}
	}

	public abstract class MonitoredMarkupExtension : DeferredMarkupExtension
	{
		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			var result = MarkupExtensionMonitor.Instance.IsInitialized ? GetValue( serviceProvider ) : Listen( serviceProvider, setter );
			return result;
		}

		protected virtual object Listen( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			MarkupExtensionMonitor.Instance.Initialized += ( sender, args ) =>
			{
				var value = GetValue( serviceProvider );
				setter.SetValue( value );
			};
			return null;
		}

		protected abstract object GetValue( IServiceProvider serviceProvider );
	}
}