using Microsoft.AspNetCore.Identity;
using System;

namespace DragonSpark.Application.AspNet.Security.Identity;

public class IdentityUser : IdentityUser<int> // TODO V2: DATA: uint
{
	public DateTimeOffset Created { get; set; }

	public DateTimeOffset? Modified { get; set; }
}