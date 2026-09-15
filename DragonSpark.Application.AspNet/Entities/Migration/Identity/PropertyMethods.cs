using DragonSpark.Model.Selection.Stores;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class PropertyMethods : ConcurrentStore<Type, MethodInfo>
{
	public static PropertyMethods Default { get; } = new();

	PropertyMethods() : this(PropertyMethod.Default) {}

	public PropertyMethods(MethodInfo method) : base(x => method.MakeGenericMethod(x)) {}
}