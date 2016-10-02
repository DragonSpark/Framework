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
		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable() ) {}
		public InstanceRepository( IEnumerable<object> items ) : base( items ) {}
		public InstanceRepository( ICollection<object> source ) : base( source ) {}

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
		// bool ISpecification.IsSatisfiedBy( object parameter ) => parameter is Type && IsSatisfiedBy( (Type)parameter );

		public object Get( Type parameter ) => GetService( parameter );
		public object Get( object parameter ) => parameter is Type ? Get( parameter ) : null;
	}
}