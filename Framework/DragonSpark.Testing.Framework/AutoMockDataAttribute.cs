using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework
{
	public class AutoMockDataAttribute : CustomizedAutoDataAttribute
	{
		public AutoMockDataAttribute() : base( typeof(AutoMoqCustomization) )
		{}
	}
}