using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class ConsoleReviewAwareMigration : IMigration
{
	readonly IMigration      _previous;
	readonly ReviewMigration _review;

	public ConsoleReviewAwareMigration(IMigration previous, ReviewMigration review)
	{
		_previous = previous;
		_review   = review;
	}

	public ValueTask Get(Stop<ushort?> parameter) => _review.Get() ? _previous.Get(parameter) : ValueTask.CompletedTask;
}