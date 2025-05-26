using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class SelectedAttach<TFrom, TTo> : Modify<TFrom> where TTo : class
{
	protected SelectedAttach(IEnlistedScopes scopes, IOperation<Stop<Edit<TFrom>>> from, Func<TFrom, TTo> select)
		: base(scopes,
		       Start.A.Selection<Stop<Edit<TFrom>>>()
		            .By.Calling(x =>
		                        {
			                        var ((editor, subject), _) = x;
			                        return new Edit<TTo>(editor, select(subject));
		                        })
		            .Terminate(AttachLocal<TTo>.Default)
		            .Operation()
		            .Append(from)) {}
}