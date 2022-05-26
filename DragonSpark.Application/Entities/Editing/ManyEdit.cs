using DragonSpark.Model.Sequences.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public readonly struct ManyEdit<T> : IEditor
{
	readonly IEditor _editor;

	public static implicit operator Memory<T>(ManyEdit<T> instance) => instance.Subject;

	public ManyEdit(IEditor editor, Leasing<T> subject)
	{
		Subject  = subject;
		_editor = editor;
	}

	public Leasing<T> Subject { get; }

	public void Dispose()
	{
		_editor.Dispose();
		Subject.Dispose();
	}

	public ValueTask Get() => _editor.Get();

	public void Add(object entity)
	{
		_editor.Add(entity);
	}

	public void Attach(object entity)
	{
		_editor.Attach(entity);
	}

	public void Update(object entity)
	{
		_editor.Update(entity);
	}

	public void Remove(object entity)
	{
		_editor.Remove(entity);
	}

	public void Clear()
	{
		_editor.Clear();
	}

	public ValueTask Refresh(object entity) => _editor.Refresh(entity);
}