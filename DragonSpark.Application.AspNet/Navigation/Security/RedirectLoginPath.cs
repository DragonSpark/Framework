using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class RedirectLoginPath : SelectedResult<string, string>
{
	public RedirectLoginPath(CurrentRootPath previous) : base(previous, LoginPath.Default) {}
}