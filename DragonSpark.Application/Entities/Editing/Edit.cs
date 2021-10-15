using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class Edit<TIn, T, TResult> : IEdit<TIn, TResult>
{
	readonly IReading<TIn, T>      _reading;
	readonly IEvaluate<T, TResult> _evaluate;

	public Edit(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
	{
		_reading  = reading;
		_evaluate = evaluate;
	}

	public async ValueTask<Edit<TResult>> Get(TIn parameter)
	{
		var (context, disposable, elements) = await _reading.Await(parameter);
		var evaluate = await _evaluate.Await(elements);
		return new(new Editor(context, disposable), evaluate);
	}
}

public readonly struct Edit<T> : IEditor
{
	readonly IEditor _context;

	public Edit(IEditor context, T subject)
	{
		Subject  = subject;
		_context = context;
	}

	public T Subject { get; }

	public ValueTask Get() => _context.Get();

	public void Add(object entity)
	{
		_context.Add(entity);
	}

	public void Attach(object entity)
	{
		_context.Attach(entity);
	}

	public void Update(object entity)
	{
		_context.Update(entity);
	}

	public void Remove(object entity)
	{
		_context.Remove(entity);
	}

	public void Dispose()
	{
		_context.Dispose();
	}

	public void Deconstruct(out IEditor context, out T subject)
	{
		context = _context;
		subject = Subject;
	}
}

public class Edit<TIn, T> : Selecting<TIn, Edit<T>>, IEdit<TIn, T>
{
	protected Edit(ISelect<TIn, ValueTask<Edit<T>>> select) : base(select) {}

	protected Edit(Func<TIn, ValueTask<Edit<T>>> select) : base(select) {}
}