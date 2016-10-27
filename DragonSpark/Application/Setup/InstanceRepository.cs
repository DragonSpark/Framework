using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class InstanceRepository : RepositoryBase<object>, IServiceRepository
	{
		readonly static Func<Type, Func<object, object>> AccountedSource = SourceAccountedValues.Defaults.Get;

		readonly Func<Type, Func<object, object>> accountedSource;
		public InstanceRepository() {}

		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable(), AccountedSource ) {}

		[UsedImplicitly]
		public InstanceRepository( IEnumerable<object> items, Func<Type, Func<object, object>> accountedSource ) : base( items )
		{
			this.accountedSource = accountedSource;
		}
		
		public virtual object GetService( Type serviceType )
		{
			var source = accountedSource( serviceType );
			var result = Yield().SelectAssigned( source ).FirstOrDefault();
			return result;
		}

		public virtual void Add( ServiceRegistration request ) => Add( request.Instance );

		public bool IsSatisfiedBy( Type parameter ) => GetService( parameter ).IsAssigned();

		public object Get( Type parameter ) => GetService( parameter );
	}
}