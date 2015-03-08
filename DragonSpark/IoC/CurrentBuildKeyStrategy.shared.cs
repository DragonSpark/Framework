using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC
{
	public class CurrentBuildKeyStrategy : BuilderStrategy
	{
		readonly List<NamedTypeBuildKey> source = new List<NamedTypeBuildKey>();

		readonly ReadOnlyCollection<NamedTypeBuildKey> stack;

		public CurrentBuildKeyStrategy()
		{
			stack = new ReadOnlyCollection<NamedTypeBuildKey>( source );
		}

		public IEnumerable<NamedTypeBuildKey> Stack
		{
			get { return stack; }
		}

		public override void PreBuildUp(IBuilderContext context)
		{
			source.Add( context.BuildKey );
			base.PreBuildUp(context);
		}

		public override void PostBuildUp(IBuilderContext context)
		{
			source.Remove( source.Last() );
			base.PostBuildUp(context);
		}
	}
}