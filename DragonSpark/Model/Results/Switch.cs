using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Results;

public class Switch : Variable<bool>, ISwitch
{
	public static implicit operator bool(Switch instance) => instance.Get();

	public Switch(bool instance = default) : base(instance) {}
}
// TODO
public readonly struct Switching : IDisposable
{
	readonly ISwitch _subject;

	public Switching(ISwitch subject) => _subject = subject;

	public void Dispose()
	{
		_subject.Down();
	}
}