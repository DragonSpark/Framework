using DragonSpark.Application.Model.Text;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginProblemPath : Formatted<string>
{
	public static LoginProblemPath Default { get; } = new();

	LoginProblemPath() : base($"{LoginProblemTemplate.Default}/{{0}}") {}
}