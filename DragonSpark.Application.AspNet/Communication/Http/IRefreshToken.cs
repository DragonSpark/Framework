using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Communication.Http;

public interface IRefreshToken : IStopAware<string, IResult>;