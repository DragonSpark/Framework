using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Selection
{
	public sealed class SelectionImplementations : GenericImplementations
	{
		public static SelectionImplementations Default { get; } = new SelectionImplementations();

		SelectionImplementations() : base(typeof(ISelect<,>)) {}
	}
}