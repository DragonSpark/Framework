using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class InstanceRepository : RepositoryBase<object>, IServiceRepository
	{
		readonly Func<Type, IParameterizedSource<object>> sourceSource;
		public InstanceRepository() {}

		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable(), SourceAccountedValues.Defaults.Get ) {}

		[UsedImplicitly]
		public InstanceRepository( IEnumerable<object> items, Func<Type, IParameterizedSource<object>> sourceSource ) : base( items )
		{
			this.sourceSource = sourceSource;
		}
		
		public virtual object GetService( Type serviceType )
		{
			var source = sourceSource( serviceType ).ToSourceDelegate();
			var result = Yield().SelectAssigned( source ).FirstOrDefault();
			return result;
		}

		public virtual void Add( ServiceRegistration request ) => Add( request.Instance );

		public bool IsSatisfiedBy( Type parameter ) => GetService( parameter ).IsAssigned();

		public object Get( Type parameter ) => GetService( parameter );
	}
}