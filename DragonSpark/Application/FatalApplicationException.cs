using System;
using System.Runtime.Serialization;

namespace DragonSpark.Application
{
	[Serializable]
	partial class FatalApplicationException
	{
		protected FatalApplicationException(
			SerializationInfo info,
			StreamingContext context ) : base( info, context )
		{}
	}
}
