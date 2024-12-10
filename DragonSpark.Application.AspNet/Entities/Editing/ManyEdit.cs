using System;
using System.Threading.Tasks;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

[method: MustDisposeResource]
public readonly struct ManyEdit<T>(IEditor editor, Leasing<T> subject) : IEditor
{
	public static implicit operator Memory<T>(ManyEdit<T> instance) => instance.Subject;

	public Leasing<T> Subject { get; } = subject;

	public void Dispose()
	{
		editor.Dispose();
		Subject.Dispose();
	}

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
}
