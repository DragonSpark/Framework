using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class CustomizationBase : ICustomization
	{
		public void Customize( IFixture fixture ) => OnCustomize( fixture );

		protected abstract void OnCustomize( IFixture fixture );
	}
}