using System;

namespace DragonSpark.Azure;

public readonly record struct MessageInput(string Message, TimeSpan? Visibility, TimeSpan? Life);