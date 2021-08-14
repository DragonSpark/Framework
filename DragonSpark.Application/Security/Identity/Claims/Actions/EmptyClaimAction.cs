using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Actions
{
	public sealed class EmptyClaimAction : Command<ClaimActionCollection>, IClaimAction
	{
		public static EmptyClaimAction Default { get; } = new EmptyClaimAction();

		EmptyClaimAction() : base(_ => {}) {}
	}
}