using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public sealed class GitHubApplicationAttribute : HostingAttribute
	{
		public GitHubApplicationAttribute() : base(A.Type<GitHubApplicationAttribute>().Assembly) {}
	}
}