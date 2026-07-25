using DragonSpark.Model.Results;

namespace DragonSpark.Reflection;

sealed class DeclaredValue<TAttribute, T> : Declared<TAttribute, T> where TAttribute : Attribute, IResult<T>
{
	public static DeclaredValue<TAttribute, T> Default { get; } = new();

	DeclaredValue() : base(Results<T>.Default.Get) {}
}