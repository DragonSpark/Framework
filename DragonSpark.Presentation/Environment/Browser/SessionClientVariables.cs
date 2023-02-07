using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class SessionClientVariables<T> : ClientVariables<T>
{
	public SessionClientVariables(ProtectedSessionStorage storage) : base(storage) {}
}