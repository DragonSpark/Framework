using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	public class CompositeClaimAction : CompositeCommand<ClaimActionCollection>, IClaimAction
	{
		public CompositeClaimAction(params ICommand<ClaimActionCollection>[] items) : base(items) {}
	}
}