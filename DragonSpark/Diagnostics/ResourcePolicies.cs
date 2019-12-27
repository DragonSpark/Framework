using Polly;
using DragonSpark.Model.Selection.Alterations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics
{
	public sealed class ResourcePolicies : Alteration<PolicyBuilder>
	{
		public static ResourcePolicies Default { get; } = new ResourcePolicies();

		ResourcePolicies() : base(x => x.Or<IOException>()
		                                .Or<HttpRequestException>()
		                                .Or<TaskCanceledException>()) {}
	}
}