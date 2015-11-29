using DragonSpark.Activation.Build;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public abstract class InjectionMemberFactory<TMember> : FactoryBase<InjectionMemberContext, TMember> where TMember : InjectionMember
	{}
}