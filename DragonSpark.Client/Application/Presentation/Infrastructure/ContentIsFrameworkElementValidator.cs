using System;
using System.Windows;
using DragonSpark.Application.Presentation.Navigation;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public class ContentIsFrameworkElementValidator : IViewValidator
	{
		public void Validate( ViewValidationContext context )
		{
			context.Content.As<FrameworkElement>().Null( () =>
			{
				throw new NotSupportedException( string.Format( "View must be a FrameworkElement.  Specified type is: {0}", context.Content.Transform( x => x.GetType() ) ) );
			} );
		}
	}
}