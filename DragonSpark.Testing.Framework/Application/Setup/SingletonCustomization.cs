using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class SingletonCustomization : CustomizationBase
	{
		readonly static MethodInvoker MethodInvoker = new MethodInvoker( SingletonQuery.Default );

		public static SingletonCustomization Default { get; } = new SingletonCustomization();
		SingletonCustomization() {}

		protected override void OnCustomize( IFixture fixture ) => fixture.Customizations.Insert( 0, MethodInvoker );
	}
}