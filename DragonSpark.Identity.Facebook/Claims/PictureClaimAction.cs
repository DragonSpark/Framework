using DragonSpark.Application.Security.Identity.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class PictureClaimAction : CustomClaimAction
	{
		public static PictureClaimAction Default { get; } = new PictureClaimAction();

		PictureClaimAction() : base(Picture.Default, "url", x => x.GetProperty("picture").GetProperty("data").GetString("url")!) {}
	}
}