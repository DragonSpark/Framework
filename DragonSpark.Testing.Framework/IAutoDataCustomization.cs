using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public interface IAutoDataCustomization : ICustomization
	{
		void Initializing( AutoData data );

		void Initialized( AutoData data );
	}
}