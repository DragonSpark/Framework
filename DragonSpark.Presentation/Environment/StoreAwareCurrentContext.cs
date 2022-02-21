using DragonSpark.Application.Security;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class StoreAwareCurrentContext : Coalesce<HttpContext>, ICurrentContext
{
	public StoreAwareCurrentContext(ContextStore first, ICurrentContext second) : base(first, second) {}
}