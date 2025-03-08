using System;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class Edit<TIn, T, TResult> : IEdit<TIn, TResult>
{
	readonly IReading<TIn, T>      _reading;
	readonly IEvaluate<T, TResult> _evaluate;

	public Edit(IReading<TIn, T> reading, IEvaluate<T, TResult> evaluate)
	{
		_reading  = reading;
		_evaluate = evaluate;
	}

	[MustDisposeResource]
	public async ValueTask<Edit<TResult>> Get(TIn parameter)
	{
		var (context, disposable, elements) = _reading.Get(parameter);
		var evaluate = await _evaluate.Off(elements);
		return new(new Editor(context, disposable), evaluate);
	}
}

[method: MustDisposeResource]
public readonly struct Edit<T>(IEditor editor, T subject) : IEditor
{
	public T Subject { get; } = subject;

	public ValueTask Get() => editor.Get();

	public void Add(object entity)
	{
		editor.Add(entity);
	}

	public void Attach(object entity)
	{
		editor.Attach(entity);
	}

	public void Update(object entity)
	{
		editor.Update(entity);
	}

	public void Remove(object entity)
	{
		editor.Remove(entity);
	}

	public void Clear()
	{
		editor.Clear();
	}

	public ValueTask Refresh(object entity) => editor.Refresh(entity);

	public void Dispose()
	{
		editor.Dispose();
	}

	public void Deconstruct([MustDisposeResource(false)] out IEditor context, out T subject)
	{
		context = editor;
		subject = Subject;
	}
}

public class Edit<TIn, T> : Selecting<TIn, Edit<T>>, IEdit<TIn, T>
{
	protected Edit(ISelect<TIn, ValueTask<Edit<T>>> select) : base(select) {}

	protected Edit(Func<TIn, ValueTask<Edit<T>>> select) : base(select) {}
}
