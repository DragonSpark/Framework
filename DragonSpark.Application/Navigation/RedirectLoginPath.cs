using DragonSpark.Model.Results;

namespace DragonSpark.Application.Navigation;

public sealed class RedirectLoginPath : SelectedResult<string, string>
{
	public RedirectLoginPath(CurrentRootPath previous) : base(previous, LoginPath.Default) {}
}