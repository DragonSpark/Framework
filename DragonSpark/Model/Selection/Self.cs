using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Model.Selection;

sealed class Self<T> : IAlteration<T>
{
	public static IAlteration<T> Default { get; } = new Self<T>();

	Self() {}

	public T Get(T parameter) => parameter;
}