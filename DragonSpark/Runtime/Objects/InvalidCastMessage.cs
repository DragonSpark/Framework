using DragonSpark.Text;

namespace DragonSpark.Runtime.Objects
{
	sealed class InvalidCastMessage<TFrom, TTo> : Message<TFrom>
	{
		public static InvalidCastMessage<TFrom, TTo> Default { get; } = new InvalidCastMessage<TFrom, TTo>();

		InvalidCastMessage() :
			base(x => $"Could not cast an object of '{x?.GetType() ?? typeof(TFrom)}' to '{typeof(TTo)}'.") {}
	}
}