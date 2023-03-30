using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Results;

public readonly struct Switching : IDisposable
{
	readonly ISwitch _subject;

	public Switching(ISwitch subject) => _subject = subject;

	public void Dispose()
	{
		_subject.Down();
	}
}