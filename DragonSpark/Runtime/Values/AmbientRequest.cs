using System;

namespace DragonSpark.Runtime.Values
{
	class AmbientRequest : IAmbientRequest
	{
		public AmbientRequest( Type requestedType, object context )
		{
			RequestedType = requestedType;
			Context = context;
		}

		public Type RequestedType { get; }

		public object Context { get; }
	}
}