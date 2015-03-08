using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.IoC
{
	public class DefaultUnityConstructorSelectorPolicy : Microsoft.Practices.Unity.ObjectBuilder.DefaultUnityConstructorSelectorPolicy
	{
		protected override IDependencyResolverPolicy CreateResolver( System.Reflection.ParameterInfo parameter )
		{
			var isOptional = parameter.IsOptional && !parameter.IsDecoratedWith<OptionalDependencyAttribute>();
			var dependencyResolverPolicy = base.CreateResolver( parameter );
			var result = isOptional ? parameter.ParameterType.IsValueType || parameter.ParameterType == typeof(string) ? (IDependencyResolverPolicy)new LiteralValueDependencyResolverPolicy( parameter.DefaultValue ) : new OptionalDependencyResolverPolicy( parameter.ParameterType ) : dependencyResolverPolicy;
			return result;
		}
	}
}