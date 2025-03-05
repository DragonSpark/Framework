using System;

namespace DragonSpark.Application.Mobile.Security.Authentication;

public readonly record struct ResumeSessionInput(string Identifier, Uri Address);