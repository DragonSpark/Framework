using DragonSpark.Activation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public abstract class InjectionMemberFactory<TMember> : FactoryBase<InjectionMemberContext, TMember> where TMember : InjectionMember
	{}
}