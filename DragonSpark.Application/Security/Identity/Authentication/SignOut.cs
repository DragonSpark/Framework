using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public sealed class SignOut : IOperation
	{
		readonly IHttpContextAccessor _accessor;

		public SignOut(IHttpContextAccessor accessor) => _accessor = accessor;

		public ValueTask Get() => _accessor.HttpContext.Verify().SignOutAsync().ToOperation();
	}
}