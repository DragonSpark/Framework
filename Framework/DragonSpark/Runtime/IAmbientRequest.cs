using System;

namespace DragonSpark.Runtime
{
	public interface IAmbientRequest
	{
		Type RequestedType { get; }

		object Context { get; }
	}

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