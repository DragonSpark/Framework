using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.IoC
{
	public class CurrentBuildKeyStrategy : BuilderStrategy
	{
		static IList<NamedTypeBuildKey> Source
		{
			get { return source ?? ( source = new List<NamedTypeBuildKey>() ); }
		}	[ThreadStatic] static IList<NamedTypeBuildKey> source;
		
		public IEnumerable<NamedTypeBuildKey> Stack
		{
			get { return stack ?? ( stack = new ReadOnlyCollection<NamedTypeBuildKey>( Source ) ); }
		}	[ThreadStatic] static ReadOnlyCollection<NamedTypeBuildKey> stack;

		public override void PreBuildUp(IBuilderContext context)
		{
			Source.Add( context.BuildKey );
			base.PreBuildUp(context);
		}

		public override void PostBuildUp(IBuilderContext context)
		{
			Source.Remove( source.Last() );
			base.PostBuildUp(context);
		}
	}

	public class CurrentContextStrategy : BuilderStrategy
	{
		static IList<IBuilderContext> Source
		{
			get { return source ?? ( source = new List<IBuilderContext>() ); }
		}	[ThreadStatic] static IList<IBuilderContext> source;
		
		public IEnumerable<IBuilderContext> Stack
		{
			get { return stack ?? ( stack = new ReadOnlyCollection<IBuilderContext>( Source ) ); }
		}	[ThreadStatic] static ReadOnlyCollection<IBuilderContext> stack;

		public override void PreBuildUp(IBuilderContext context)
		{
			Source.Add( context );
			base.PreBuildUp(context);
		}

		public override void PostBuildUp(IBuilderContext context)
		{
			Source.Remove( Source.Last() );
			base.PostBuildUp(context);
		}
	}
}