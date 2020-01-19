using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class Parameters : Select<ConstructorInfo, Array<ParameterInfo>>
	{
		public static Parameters Default { get; } = new Parameters();

		Parameters() : base(x => x.GetParameters()) {}
	}
}