using DragonSpark.Sources.Parameterized;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public interface IDefaultValueProvider : IParameterizedSource<PropertyInfo, object> {}
}