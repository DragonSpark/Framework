using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class InstanceRepository : RepositoryBase<object>, IServiceRepository
	{
		public InstanceRepository() {}

		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable() ) {}
		public InstanceRepository( IEnumerable<object> items ) : base( items ) {}
		
		public virtual object GetService( Type serviceType ) => Get( serviceType, o => o.Value() );

		T Get<T>( Type serviceType, Func<object, T> projection )
		{
			var specification = TypeAssignableSpecification.Defaults.Get( serviceType );
			foreach ( var item in Yield() )
			{
				var parameter = ( item as IServiceAware )?.ServiceType ?? item.GetType();
				if ( specification.IsSatisfiedBy( parameter ) )
				{
					return projection( item );
				}
			}
			return default(T);
		}

		public virtual void Add( InstanceRegistrationRequest request ) => Add( request.Instance );

		public bool IsSatisfiedBy( Type parameter ) => Get( parameter, o => true );
		
		public object Get( Type parameter ) => GetService( parameter );
	}
}