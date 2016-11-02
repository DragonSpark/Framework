using DragonSpark.Sources.Scopes;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class CurrentFixture : Scope<IFixture>
	{
		public static CurrentFixture Default { get; } = new CurrentFixture();
		CurrentFixture() {}
	}
}