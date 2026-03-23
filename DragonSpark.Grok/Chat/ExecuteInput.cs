using System.Collections.Generic;

namespace DragonSpark.Grok.Chat;

public readonly record struct ExecuteInput(string Name, Dictionary<string, object> Arguments);