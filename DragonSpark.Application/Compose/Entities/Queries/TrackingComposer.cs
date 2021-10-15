using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class TrackingComposer<TIn, T> where T : class
{
	readonly QueryComposer<TIn, T> _subject;

	public TrackingComposer(QueryComposer<TIn, T> subject) => _subject = subject;

	public QueryComposer<TIn, T> Off() => _subject.Select(x => x.AsNoTracking());

	public QueryComposer<TIn, T> Unique() => _subject.Select(x => x.AsNoTrackingWithIdentityResolution());
}