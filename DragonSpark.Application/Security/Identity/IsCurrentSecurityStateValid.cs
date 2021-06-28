using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class IsCurrentSecurityStateValid : Resulting<bool>
	{
		public IsCurrentSecurityStateValid(IHasValidSecurityState valid, ICurrentPrincipal current)
			: base(valid.Then().Bind(current)) {}
	}
}