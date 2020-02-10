using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public interface IClaims : IArray<ExternalLoginInfo, Claim> {}
}