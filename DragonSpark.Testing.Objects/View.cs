using DragonSpark.Model.Sequences;

namespace DragonSpark.Testing.Objects;

sealed class View : Instances<string>
{
	public static View Default { get; } = new();

	View() : base(Data.Default) {}
}