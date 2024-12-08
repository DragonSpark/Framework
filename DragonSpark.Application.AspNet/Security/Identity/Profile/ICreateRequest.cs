using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Profile;

public interface ICreateRequest : ISelecting<ExternalLoginInfo, CreateRequestResult>;