using DragonSpark.Application;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content;
using System;

namespace DragonSpark.Presentation.Compose;

public class ActiveContentComposer<T> : IResult<IActiveContent<T>>
{
	readonly IActiveContent<T> _subject;

	public ActiveContentComposer(IActiveContent<T> subject) => _subject = subject;

	public ActiveContentComposer<T> Handle(IExceptions exceptions, Type reportedType)
		=> new(new ActiveContentAdapter<T>(_subject, A.Result(_subject).Then().Handle(exceptions, reportedType).Out()));

	public IActiveContent<T> Get() => _subject;
}