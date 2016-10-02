using System.Composition;

namespace DragonSpark.Testing.Objects
{
	[Export( typeof(IInterface) )]
	public class Class : IInterface {}
}