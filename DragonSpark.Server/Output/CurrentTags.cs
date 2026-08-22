using DragonSpark.Application.AspNet.Security;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

sealed class CurrentTags : ICurrentTags
{
	readonly ICurrentContext _context;

	public CurrentTags(ICurrentContext context) => _context = context;

	public ICollection<string>? Get() => _context.Get().Features.Get<IOutputCacheFeature>()?.Context.Tags;
}