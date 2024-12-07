using System;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

public readonly record struct KeyCodeView(string Key, Uri Location);