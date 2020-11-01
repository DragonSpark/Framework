using System.Collections.Generic;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class VisitedKey : ValidatorKey<HashSet<object>>
	{
		public static VisitedKey Default { get; } = new VisitedKey();

		VisitedKey() : base(new object()) {}
	}
}