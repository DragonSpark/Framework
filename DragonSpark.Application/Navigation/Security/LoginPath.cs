namespace DragonSpark.Application.Navigation.Security;

public sealed class LoginPath : TemplatedPath
{
	public static LoginPath Default { get; } = new LoginPath();

	LoginPath() : base(LoginPathTemplate.Default) {}
}