using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.Setup
{
	public class InstanceRepository : RepositoryBase<object>, IServiceRepository
	{
		readonly static Func<Type, Func<Type, bool>> Specifications = TypeAssignableSpecification.Delegates.Get;
		readonly static Func<Type, Func<object, object>> AccountedSource = SourceAccountedAlteration.Defaults.Get;
		readonly static Func<Type, IEnumerable<Type>> Types = SourceAccountedTypes.Default.GetEnumerable;

		readonly Func<Type, Func<Type, bool>> specifications;
		readonly Func<Type, IEnumerable<Type>> types;
		readonly Func<Type, Func<object, object>> accountedSource;
		public InstanceRepository() : this( Items<object>.Default ) {}

		public InstanceRepository( params object[] instances ) : this( instances.AsEnumerable(), Specifications, Types, AccountedSource ) {}

		[UsedImplicitly]
		public InstanceRepository( IEnumerable<object> items, Func<Type, Func<Type, bool>> specifications, Func<Type, IEnumerable<Type>> types, Func<Type, Func<object, object>> accountedSource ) : base( items )
		{
			this.specifications = specifications;
			this.types = types;
			this.accountedSource = accountedSource;
		}
		
		public virtual object GetService( Type serviceType ) => Yield().SelectAssigned( accountedSource( serviceType ) ).FirstOrDefault();

		public virtual void Add( ServiceRegistration request ) => Add( request.Instance );

		public bool IsSatisfiedBy( Type parameter ) => Yield().SelectTypes().SelectMany( types ).Any( specifications( parameter ) );

		public object Get( Type parameter ) => GetService( parameter );
	}
}