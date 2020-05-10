using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public sealed class Name : Select<ClaimsPrincipal, string>
	{
		public static Name Default { get; } = new Name();

		Name() : base(x => x.Identity.Name.Verify()) {}
	}
}