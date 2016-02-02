using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;

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
			var monitor = new AssociatedMonitor( serviceProvider.Get<IRootObjectProvider>().RootObject ).Item;
			var result = monitor.IsInitialized ? GetValue( serviceProvider ) : Listen( monitor, serviceProvider, setter );
			return result;
		}

		protected virtual object Listen( MarkupExtensionMonitor monitor, IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			monitor.Initialized += ( sender, args ) =>
			{
				var value = GetValue( serviceProvider );
				setter.SetValue( value );
			};
			return null;
		}

		protected abstract object GetValue( IServiceProvider serviceProvider );
	}
}