using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Compose.Model.Selection;

public class AlterationComposer<T> : Composer<T, T>, IResult<IAlteration<T>>
{
	public static implicit operator Func<T, T>(AlterationComposer<T> instance) => instance.Get().Get;

	readonly IAlteration<T> _subject;

	public AlterationComposer(IAlteration<T> subject) : base(subject) => _subject = subject;

	public new IAlteration<T> Get() => _subject;
}