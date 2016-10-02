using DragonSpark.Sources;

namespace DragonSpark.Testing.Objects
{
	public class Constructor : SourceBase<ClassWithParameter>
	{
		public override ClassWithParameter Get() => new ClassWithParameter( this );
	}
}