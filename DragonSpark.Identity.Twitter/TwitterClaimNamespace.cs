﻿namespace DragonSpark.Identity.Twitter
{
	public sealed class TwitterClaimNamespace : Text.Text
	{
		public static TwitterClaimNamespace Default { get; } = new TwitterClaimNamespace();

		TwitterClaimNamespace() : base("urn:twitter") {}
	}
}