using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Selection;

public sealed class ImplementsSelection : ImplementsGenericType
{
	public static ImplementsSelection Default { get; } = new();

	ImplementsSelection() : base(typeof(ISelect<,>)) {}
}