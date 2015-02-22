using System;

namespace DragonSpark.Application.Presentation
{
	public interface IViewTypeLocator
	{
		Type DetermineType( Type modelType, object context = null );
	}
}