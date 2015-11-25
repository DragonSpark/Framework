using System;

namespace DragonSpark.Runtime
{
	public interface ISingletonLocator
	{
		object Locate( Type type );
	}
}