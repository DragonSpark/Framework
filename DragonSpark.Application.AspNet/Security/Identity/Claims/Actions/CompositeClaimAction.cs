using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Actions;

public class CompositeClaimAction : Commands<ClaimActionCollection>, IClaimAction
{
	public CompositeClaimAction(params ICommand<ClaimActionCollection>[] items) : base(items) {}
}