using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public interface IRequesting<T> : ISelecting<Request<T>, IActionResult> {}
}