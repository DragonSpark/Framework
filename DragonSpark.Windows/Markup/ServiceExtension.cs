using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;
using System;
using Type = System.Type;

namespace DragonSpark.Windows.Markup
{
	public class ServiceExtension : MarkupExtensionBase
	{
		public ServiceExtension() {}

		public ServiceExtension( Type serviceType )
		{
			ServiceType = serviceType;
		}

		[NotNull]
		public Type ServiceType { [return: NotNull]get; set; }

		[Service, NotNull]
		public IServiceProvider Locator { [return: NotNull]get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) => 
			Locator.GetService( ServiceType );
	}
}