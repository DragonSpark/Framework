using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IExternalChallenge : ISelect<PerformExternalLoginInput, IResult>;