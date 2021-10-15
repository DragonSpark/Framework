namespace DragonSpark.Application.Navigation;

public sealed class LoginPath : TemplatedPath
{
	public static LoginPath Default { get; } = new LoginPath();

	LoginPath() : base(LoginPathTemplate.Default) {}
}