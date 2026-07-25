using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public class IdentityUser : IdentityUser<uint>
{
	public DateTimeOffset Created { get; set; }

	public DateTimeOffset? Modified { get; set; }
}