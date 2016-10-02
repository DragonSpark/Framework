using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public struct SingletonRequest
	{
		public SingletonRequest( Type requestedType, PropertyInfo candidate )
		{
			RequestedType = requestedType;
			Candidate = candidate;
		}

		public Type RequestedType { get; }
		public PropertyInfo Candidate { get; }
	}
}