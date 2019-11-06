using Refit;
using System.Threading.Tasks;

namespace DragonSpark.Services.Security
{
	interface IAuthentication
	{
		[Get("/.auth/me")]
		Task<AuthenticationInformation[]> Current();
	}
}