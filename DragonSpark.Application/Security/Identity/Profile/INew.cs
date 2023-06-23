using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface INew<T> : ISelecting<ExternalLoginInfo, T> where T : class {}