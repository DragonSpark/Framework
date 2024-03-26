using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Entities.Editing;

public class SelectedAttach<TFrom, TTo> : Modify<TFrom> where TTo : class
{
	protected SelectedAttach(IEnlistedScopes scopes, IOperation<Edit<TFrom>> from, Func<TFrom, TTo> select)
		: base(scopes,
		       Start.A.Selection<Edit<TFrom>>()
		            .By.Calling(x =>
		                        {
			                        var (editor, subject) = x;
			                        return new Edit<TTo>(editor, select(subject));
		                        })
		            .Terminate(AttachLocal<TTo>.Default)
		            .Operation()
		            .Append(from)) {}
}