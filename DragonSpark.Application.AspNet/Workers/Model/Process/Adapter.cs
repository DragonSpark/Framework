using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class Adapter<T, TSubject> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<TSubject> _subject;
	readonly Func<T, TSubject>    _select;

	protected Adapter(IStopAware<TSubject> subject, Func<T, TSubject> select)
	{
		_subject = subject;
		_select  = select;
	}

	public ValueTask Get(Stop<T> parameter) => _subject.Get(new(_select(parameter), parameter));
}

