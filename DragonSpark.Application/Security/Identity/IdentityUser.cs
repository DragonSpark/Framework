using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.Security.Identity;

public class IdentityUser : IdentityUser<int>
{
	public DateTimeOffset Created { get; set; }

	public DateTimeOffset? Modified { get; set; }

	public object Reference() => MemberwiseClone();
}