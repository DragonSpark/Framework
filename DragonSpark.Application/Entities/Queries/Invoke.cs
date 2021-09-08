using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	public class Invoke<TIn, T> : IInvoke<TIn, T>
	{
		readonly IInvocations  _invocations;
		readonly IForm<TIn, T> _form;

		public Invoke(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(invocations, new Form<TIn, T>(expression)) {}

		public Invoke(IInvocations invocations, IForm<TIn, T> form)
		{
			_invocations = invocations;
			_form        = form;
		}

		public async ValueTask<Invoke<T>> Get(TIn parameter)
		{
			var (context, session) = _invocations.Get();
			var form = _form.Get(new(context, parameter));

			return new(context, await session.Get(), form);
		}
	}
}