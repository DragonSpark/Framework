using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using PostSharp.Extensibility;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public abstract class AssemblySourceBase : FactoryBase<Assembly[]>
	{
		[Freeze( AttributeInheritance = MulticastInheritance.Multicast, AttributeTargetMemberAttributes = MulticastAttributes.Instance )]
		protected abstract override Assembly[] CreateItem();
	}

	public class AggregateAssemblyFactory : AggregateFactory<Assembly[]>, IAssemblyProvider
	{
		public AggregateAssemblyFactory( IFactory<Assembly[]> primary, params ITransformer<Assembly[]>[] transformers ) : base( primary, transformers ) {}

		public AggregateAssemblyFactory( Func<Assembly[]> primary, params Func<Assembly[], Assembly[]>[] transformers ) : base( primary, transformers ) {}
	}
}