using System;

namespace DragonSpark.Azure.Messaging.Messages;

public readonly record struct SendInput(TimeSpan? Life = null, TimeSpan? Visibility = null);