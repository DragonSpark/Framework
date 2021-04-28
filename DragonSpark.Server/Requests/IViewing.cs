using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public interface IViewing<T> : ISelecting<View<T>, IActionResult> {}

	public interface IViewing : ISelecting<View, IActionResult> {}
}