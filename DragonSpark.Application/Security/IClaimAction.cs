using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security {
	public interface IClaimAction : ICommand<ClaimActionCollection> {}
}