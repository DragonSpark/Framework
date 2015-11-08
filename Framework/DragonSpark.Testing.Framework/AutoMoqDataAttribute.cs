using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework
{
	public class AutoMoqDataAttribute : CustomizedAutoDataAttribute
	{
		public AutoMoqDataAttribute() : base( typeof(AutoMoqCustomization) )
		{}
	}
}
