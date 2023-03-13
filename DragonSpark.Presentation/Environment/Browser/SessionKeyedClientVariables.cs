using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class SessionKeyedClientVariables<T> : KeyedClientVariables<T>
{
	public SessionKeyedClientVariables(ProtectedSessionStorage storage) : base(storage) {}
}