using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Security.Identity.Model;

public interface IAuthenticateRequest : ISelecting<Challenged, IActionResult?>;