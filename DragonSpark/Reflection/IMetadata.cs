using System.Reflection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection
{
	public interface IMetadata<T> : IConditional<ICustomAttributeProvider, Array<T>> {}
}