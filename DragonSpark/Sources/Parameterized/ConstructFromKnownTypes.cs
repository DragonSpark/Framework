using DragonSpark.Sources.Scopes;
using DragonSpark.TypeSystem;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class ConstructFromKnownTypes<T> : ParameterizedSingletonScope<object, T>
	{
		public static ConstructFromKnownTypes<T> Default { get; } = new ConstructFromKnownTypes<T>();
		ConstructFromKnownTypes() : base( o => new DefaultImplementation().ToSingleton() ) {}

		sealed class DefaultImplementation : ParameterConstructedCompositeFactory<T>
		{
			public DefaultImplementation() : base( KnownTypes.Default.Get<T>().ToArray() ) {}
		}
	}
}