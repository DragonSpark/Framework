using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public interface IQuerying<T> : ISelecting<Query<T>, IActionResult> {}

	public interface IQuerying : ISelecting<Query, IActionResult> {}
}