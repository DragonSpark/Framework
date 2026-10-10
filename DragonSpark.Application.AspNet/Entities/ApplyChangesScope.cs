using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities;

public readonly struct ApplyChangesScope : IDisposable
{
	readonly ChangeTracker _subject;
	readonly bool          _value;

	public ApplyChangesScope(ChangeTracker subject) : this(subject, subject.AutoDetectChangesEnabled) {}

	public ApplyChangesScope(ChangeTracker subject, bool value)
	{
		_subject                         = subject;
		_value                           = value;
		subject.AutoDetectChangesEnabled = false;
	}

	public void Dispose()
	{
		_subject.AutoDetectChangesEnabled = _value;
	}
}