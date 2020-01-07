using DragonSpark.Model;
using DragonSpark.Runtime.Invocation;

namespace DragonSpark.Runtime.Objects
{
	sealed class Pairing : Invocation1<string, object, Pair<string, object>>
	{
		public Pairing(string parameter) : base(Pairs.Create, parameter) {}
	}
}