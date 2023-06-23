using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests;

public interface IQuerying<T> : ISelecting<Query<T>, IActionResult> {}

public interface IQuerying : ISelecting<Query, IActionResult> {}