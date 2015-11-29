using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Windows.Setup
{
	[ContentProperty( "Handlers" )]
	public class ExceptionPolicyEntry : MarkupExtension
	{
		public Type ExceptionType { get; set; }

		public PostHandlingAction Action { get; set; }

		public Collection<IExceptionHandler> Handlers
		{
			get { return handlers; }
		}	readonly Collection<IExceptionHandler> handlers = new Collection<IExceptionHandler>();

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry( ExceptionType, Action, Handlers );
			return result;
		}
	}
}