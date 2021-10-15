using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class ReportAwareResult<T> : IResulting<T>
{
	readonly AwaitOf<T> _subject;
	readonly Action     _report;

	public ReportAwareResult(AwaitOf<T> subject, Action report)
	{
		_subject = subject;
		_report  = report;
	}

	public async ValueTask<T> Get()
	{
		try
		{
			return await _subject();
		}
		finally
		{
			_report();
		}
	}
}