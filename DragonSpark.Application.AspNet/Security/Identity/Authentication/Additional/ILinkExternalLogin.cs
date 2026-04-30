using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface ILinkExternalLogin : ISelecting<string, IResult>;