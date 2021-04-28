using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public interface IQuerying<T> : ISelecting<Query<T>, IActionResult> {}
}