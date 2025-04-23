using System;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

public readonly record struct ResumeSessionInput(string Identifier, Uri Address);