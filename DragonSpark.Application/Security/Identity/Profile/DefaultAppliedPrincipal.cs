using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Profile {
	public sealed class DefaultAppliedPrincipal : AppliedPrincipal
	{
		public static DefaultAppliedPrincipal Default { get; } = new DefaultAppliedPrincipal();

		DefaultAppliedPrincipal() : base(new Dictionary<string, string>()) {}
	}
}