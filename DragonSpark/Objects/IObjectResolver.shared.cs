using System;

namespace DragonSpark.Objects
{
	public interface IObjectResolver
	{
		event EventHandler<ObjectResolvedEventArgs> Resolved;
		object Resolve( object key );
	}
}