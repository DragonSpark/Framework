namespace DragonSpark.Grok;

public readonly record struct ConfigureClientInput(IServiceProvider Services, HttpClient Subject);