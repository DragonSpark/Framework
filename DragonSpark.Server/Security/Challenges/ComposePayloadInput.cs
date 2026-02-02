namespace DragonSpark.Server.Security.Challenges;

public readonly record struct ComposePayloadInput(string Contents, string Signature, string Expected);