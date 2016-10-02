using DragonSpark.Sources;

namespace DragonSpark.Aspects.Implementations
{
	public sealed class Descriptors : ItemSource<IDescriptor>
	{
		public static Descriptors Default { get; } = new Descriptors();
		Descriptors() : base( ParameterizedSourceDescriptor.Default, SpecificationDescriptor.Default ) {}
	}
}