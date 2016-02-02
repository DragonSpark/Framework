using System;
using System.Windows.Markup;
using System.Xaml;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Markup
{
	public class ActivateExtension : MarkupExtension
	{
		readonly Type type;

		public ActivateExtension( [Required]Type type )
		{
			this.type = type;
		}

		public override object ProvideValue( IServiceProvider serviceProvider ) => Activator.CreateInstance( type );
	}

	public abstract class MonitoredMarkupExtension : DeferredMarkupExtension
	{
		protected override object BeginProvideValue( IServiceProvider serviceProvider, IMarkupTargetValueSetter setter )
		{
			var monitor = serviceProvider.Get<IRootObjectProvider>().RootObject.AsTo<ISetup, MarkupExtensionMonitor>( setup => new AssociatedMonitor( setup ).Item );
			var result = monitor.With( x => x.IsInitialized, () => true ) ? GetValue( serviceProvider ) : Listen( monitor, serviceProvider, setter );
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