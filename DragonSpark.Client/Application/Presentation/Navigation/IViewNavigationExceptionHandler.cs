using System;

namespace DragonSpark.Application.Presentation.Navigation
{
	public interface IViewNavigationExceptionHandler
	{
		Uri Handle( Exception e, Uri uri );
	}
}