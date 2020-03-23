﻿namespace DragonSpark.Application.Security.Identity
{
	public sealed class DisplayName : Text.Text
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
	}
}