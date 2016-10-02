using System;
using System.Reflection;

namespace DragonSpark.Composition
{
	public struct AppliedExport
	{
		public AppliedExport( Type subject, MemberInfo location, Type exportAs )
		{
			Subject = subject;
			Location = location;
			ExportAs = exportAs;
		}

		public Type Subject {get; }

		public MemberInfo Location { get; }

		public Type ExportAs { get; }
	}
}