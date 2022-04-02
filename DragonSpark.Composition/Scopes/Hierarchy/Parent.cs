using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class Parent<T> : IParent<T> where T : notnull
{
	readonly IParentServiceProvider _parent;

	public Parent(IParentServiceProvider parent) => _parent = parent;

	public T Get() => _parent.GetRequiredService<T>();
}