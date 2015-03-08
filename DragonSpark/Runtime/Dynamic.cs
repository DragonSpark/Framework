using System;
using System.Runtime.Serialization;

namespace DragonSpark.Runtime
{
	[Serializable]
	public sealed partial class ParseException
	{
		ParseException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
