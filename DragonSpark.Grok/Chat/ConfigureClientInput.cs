using System;
using System.Net.Http;

namespace DragonSpark.Grok.Chat;

public readonly record struct ConfigureClientInput(IServiceProvider Services, HttpClient Subject);