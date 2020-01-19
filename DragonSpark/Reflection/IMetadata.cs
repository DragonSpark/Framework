using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Reflection
{
	public interface IMetadata<T> : IConditional<ICustomAttributeProvider, Array<T>> {}
}