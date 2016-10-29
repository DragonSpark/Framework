using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class InstanceRepository : RepositoryBase<object>, IServiceRepository
	{
		readonly static Func<Type, Func<object, bool>> Specifications = SourceAccountedTypes.Specifications.Get;
		readonly static Func<Type, Func<object, object>> AccountedSource = SourceAccountedAlteration.Defaults.Get;

		readonly Func<Type, Func<object, bool>> specifications;
		readonly Func<Type, Func<object, object>> accountedSource;
		public InstanceRepository() : this( Items<object>.Default ) {}

		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable(), Specifications, AccountedSource ) {}

		[UsedImplicitly]
		public InstanceRepository( IEnumerable<object> items, Func<Type, Func<object, bool>> specifications, Func<Type, Func<object, object>> accountedSource ) : base( items )
		{
			this.specifications = specifications;
			this.accountedSource = accountedSource;
		}
		
		public virtual object GetService( Type serviceType ) => Yield().SelectAssigned( accountedSource( serviceType ) ).FirstOrDefault();

		public virtual void Add( ServiceRegistration request ) => Add( request.Instance );

		public bool IsSatisfiedBy( Type parameter ) => Yield().Any( specifications( parameter ) );

		public object Get( Type parameter ) => GetService( parameter );
	}
}