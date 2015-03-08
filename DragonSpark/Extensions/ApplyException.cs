using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace DragonSpark.Extensions
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
	partial class ApplyException
	{
		protected ApplyException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}
	}
}
