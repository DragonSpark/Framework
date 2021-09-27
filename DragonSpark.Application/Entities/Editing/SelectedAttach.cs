using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Editing
{
	public class SelectedAttach<TContext, TFrom, TTo> : ContextualModify<TContext, TFrom>
		where TContext : DbContext where TTo : class
	{
		protected SelectedAttach(IContexts<TContext> contexts, IOperation<Edit<TFrom>> from, Func<TFrom, TTo> select)
			: base(contexts,
			       Start.A.Selection<Edit<TFrom>>()
			            .By.Calling(x =>
			                        {
				                        var (editor, subject) = x;
				                        return new Edit<TTo>(editor, select(subject));
			                        })
			            .Terminate(Attach<TTo>.Default)
			            .Operation()
			            .Append(from)) {}
	}
}