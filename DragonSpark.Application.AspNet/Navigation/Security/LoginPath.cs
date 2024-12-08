using DragonSpark.Application.Navigation;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginPath : TemplatedPath
{
	public static LoginPath Default { get; } = new();

	LoginPath() : base(LoginPathTemplate.Default) {}
}