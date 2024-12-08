using System;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

public readonly record struct KeyCodeView(string Key, Uri Location);