using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class PlatformAgents : Instances<string>
{
	public static PlatformAgents Default { get; } = new();

	PlatformAgents()
		: base("Googlebot", "Bingbot", "facebookexternalhit", "Facebot", "Twitterbot", "Pinterest", "LinkedInBot",
		       "Discordbot", "Slackbot", "Applebot") {}
}