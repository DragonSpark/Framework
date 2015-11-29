using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Testing.TestObjects.IoC
{
	public interface ISpyPolicy : IBuilderPolicy
	{
		bool Enabled { get; set; }
	}
}