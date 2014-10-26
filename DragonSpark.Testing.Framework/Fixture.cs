using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public static class Fixture
	{
		static readonly string Key = typeof(IFixture).AssemblyQualifiedName;

		public static IFixture Current
		{
			get { return ThreadLocalStorage.Get<IFixture>( Key ); }
			set { ThreadLocalStorage.Set( Key, value ); }
		}
	}
}