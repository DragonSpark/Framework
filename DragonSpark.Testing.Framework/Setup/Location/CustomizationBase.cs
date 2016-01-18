using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public abstract class CustomizationBase : ICustomization
	{
		void ICustomization.Customize( IFixture fixture ) => Customize( fixture );

		protected abstract void Customize( IFixture fixture );
	}
}