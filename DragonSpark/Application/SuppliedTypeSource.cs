using DragonSpark.Sources;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application
{
	public class SuppliedTypeSource : ItemSource<Type>, ITypeSource
	{
		public SuppliedTypeSource() {}
		public SuppliedTypeSource( params Type[] items ) : base( items ) {}
		public SuppliedTypeSource( IEnumerable<Type> items ) : base( items ) {}
	}
}