using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Results;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public sealed class RequestHeader : IResult<ICollection<object>>
{
	readonly ICurrentBearer _bearer;

	public RequestHeader(ICurrentBearer bearer) => _bearer = bearer;

	public ICollection<object> Get() => [new { Authorization = $"Bearer {_bearer.Get()}" }];
}