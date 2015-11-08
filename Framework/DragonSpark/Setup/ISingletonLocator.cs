using System;

namespace DragonSpark.Setup
{
	public interface ISingletonLocator
	{
		object Locate( Type type );
	}
}