using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Model;

sealed class Object<T> : ISelect<T, object> where T : class
{
	public static Object<T> Default { get; } = new();

	Object() {}

	public object Get(T parameter) => parameter;
}