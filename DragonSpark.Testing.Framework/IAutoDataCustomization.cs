using System.Reflection;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public interface IAutoDataCustomization : ICustomization
	{
		void Initialize( AutoData data );

		void Before( AutoData data );

		void After( AutoData data );
	}
}