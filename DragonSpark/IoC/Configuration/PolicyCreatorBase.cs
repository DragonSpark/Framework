using DragonSpark.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC.Configuration
{
	public abstract class PolicyCreatorBase : IInstanceSource<IBuilderPolicy>
	{
		public IBuilderPolicy PolicyInstance
		{
			get { return instance ?? ( instance = CreateInstance() ); }
		}	IBuilderPolicy instance;

		IBuilderPolicy IInstanceSource<IBuilderPolicy>.Instance
		{
			get { return PolicyInstance; }
		}

		protected virtual IBuilderPolicy Instance
		{
			get { return PolicyInstance; }
		}

		protected abstract IBuilderPolicy CreateInstance();
	}
}