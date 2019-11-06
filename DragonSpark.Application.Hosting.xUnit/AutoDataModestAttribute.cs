namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class AutoDataModestAttribute : AutoDataAttribute
	{
		public AutoDataModestAttribute() : base(OptionalParameterAlteration.Default) {}
	}
}