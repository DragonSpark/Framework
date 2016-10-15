using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public abstract class DefaultValueProviderBase : ParameterizedSourceBase<PropertyInfo, object>, IDefaultValueProvider {}
}