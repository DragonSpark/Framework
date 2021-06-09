using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IAuthenticationRequest : ISelecting<Challenged, IActionResult?> {}
}