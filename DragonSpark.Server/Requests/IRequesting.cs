using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests;

public interface IRequesting<T> : ISelecting<Request<T>, IActionResult>;