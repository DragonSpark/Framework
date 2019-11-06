using DragonSpark.Model.Sequences;
using DragonSpark.Reflection;

namespace DragonSpark.Testing.Objects
{
	sealed class Numbers : ArrayStore<uint, int>
	{
		public static Numbers Default { get; } = new Numbers();

		Numbers() : base(AllNumbers.Default.ToDelegate().To(I<ClassicTake<int>>.Default).Result().Get) {}
	}
}