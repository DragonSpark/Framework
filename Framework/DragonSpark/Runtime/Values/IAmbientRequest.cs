using System;

namespace DragonSpark.Runtime.Values
{
	public interface IAmbientRequest
	{
		Type RequestedType { get; }

		object Context { get; }
	}
}