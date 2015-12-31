using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Aspects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Windows.Setup
{
	[ContentProperty( "Handlers" )]
	public class ExceptionPolicyEntry : MarkupExtension
	{
		[OfType( typeof(Exception) )]
		public Type ExceptionType { get; set; }

		public PostHandlingAction Action { get; set; }

		public Collection<IExceptionHandler> Handlers { get; } = new Collection<IExceptionHandler>();

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry( ExceptionType, Action, Handlers );
			return result;
		}
	}
}