using DragonSpark.Model.Results;

namespace DragonSpark.Application.Navigation
{
	public sealed class RedirectLoginPath : DelegatedSelection<string, string>
	{
		public RedirectLoginPath(CurrentRootPath parameter) : base(LoginPath.Default, parameter) {}
	}
}