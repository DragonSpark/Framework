using System;

namespace DragonSpark.Identity.DeviantArt.Api;

public record AccessToken(string Token, DateTimeOffset Expires);