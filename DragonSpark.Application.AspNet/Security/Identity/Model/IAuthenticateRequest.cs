using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public interface IAuthenticateRequest : IStopAware<Challenged, IActionResult?>;