namespace DragonSpark.Testing.Framework
{
	public class AutoDataAttribute : CustomizedAutoDataAttribute
	{
		public AutoDataAttribute() : base( typeof(FixtureContextCustomization), typeof(ServiceLocationCustomization) )
		{}
	}
}