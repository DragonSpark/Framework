using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.IoC
{
	public class CurrentBuildKeyStrategy : BuilderStrategy
	{
		readonly IList<NamedTypeBuildKey> source = new List<NamedTypeBuildKey>();
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

	public class CurrentContextStrategy : BuilderStrategy
	{
		readonly IList<IBuilderContext> source = new List<IBuilderContext>();
		readonly ReadOnlyCollection<IBuilderContext> stack;

		public CurrentContextStrategy()
		{
			stack = new ReadOnlyCollection<IBuilderContext>( source );
		}

		public IEnumerable<IBuilderContext> Stack
		{
			get { return stack; }
		}

		public override void PreBuildUp(IBuilderContext context)
		{
			source.Add( context );
			base.PreBuildUp(context);
		}

		public override void PostBuildUp(IBuilderContext context)
		{
			source.Remove( source.Last() );
			base.PostBuildUp(context);
		}
	}
}