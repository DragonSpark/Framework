using Refit;
using System.Threading.Tasks;

namespace DragonSpark.Server.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		Task<AuthenticationInformation[]> Current();
	}
}