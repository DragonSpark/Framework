using DragonSpark.Activation.FactoryModel;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity.Utility;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[PSerializable, AttributeUsage( AttributeTargets.Method ), LinesOfCodeAvoided( 1 )]
	public class BuildUp : OnMethodBoundaryAspect
	{
		public sealed override void OnEntry( MethodExecutionArgs args )
		{
			base.OnEntry( args );

			ObjectBuilder.Instance.BuildUp( args.Instance );
		}

		class ObjectBuilder
		{
			readonly IBuildPropertyRepository repository;
			readonly IFactory<DefaultValueParameter, object> factory;
			public static ObjectBuilder Instance { get; } = new ObjectBuilder();

			ObjectBuilder() : this( BuildPropertyRepository.Instance ) {}

			ObjectBuilder( IBuildPropertyRepository repository ) : this( repository, DefaultPropertyValueFactory.Instance ) {}

			ObjectBuilder( [Required] IBuildPropertyRepository repository, [Required] IFactory<DefaultValueParameter, object> factory )
			{
				this.repository = repository;
				this.factory = factory;
			}

			public void BuildUp( object target ) => 
				repository.GetProperties( target ).Each( property => factory.Create( property ).With( o => { repository.MarkBuilt( property.Assign( o ) ); } ) );
		}

		class BuildablePropertyCollectionFactory : FactoryBase<object, ICollection<PropertyInfo>>
		{
			public static BuildablePropertyCollectionFactory Instance { get; } = new BuildablePropertyCollectionFactory();

			protected override ICollection<PropertyInfo> CreateItem( object parameter )
			{
				var result = parameter.GetType().GetPropertiesHierarchical()
					.Where( DefaultValuePropertySpecification.Instance.IsSatisfiedBy )
					// .Where( x => Equals( GetValue( parameter, x ), x.PropertyType.Adapt().GetDefaultValue() ) )
					.ToList();
				return result;
			}
		}

		interface IBuildPropertyRepository
		{
			IEnumerable<DefaultValueParameter> GetProperties( object instance );

			void MarkBuilt( DefaultValueParameter property );
		}

		class BuildPropertyRepository : IBuildPropertyRepository
		{
			public static BuildPropertyRepository Instance { get; } = new BuildPropertyRepository();

			class IsBuilt : ConnectedValue<bool>
			{
				public IsBuilt( object instance ) : base( instance, typeof(IsBuilt) ) {}
			}

			class Properties : ConnectedValue<ICollection<PropertyInfo>>
			{
				public Properties( object instance ) : base( instance, typeof(Properties), () => BuildablePropertyCollectionFactory.Instance.Create( instance ) ) {}
			}

			public IEnumerable<DefaultValueParameter> GetProperties( object instance ) =>
				!new IsBuilt( instance ).Item ? new Properties( instance ).Item.Select( info => new DefaultValueParameter( instance, info ) ).ToArray() : Default<DefaultValueParameter>.Items;

			public void MarkBuilt( DefaultValueParameter property )
			{
				var collection = new Properties( property.Instance ).Item;
				collection.Remove( property.Metadata );
				collection.Any().IsFalse( () => new IsBuilt( property.Instance ).Assign( true ) );
			}
		}
	}
}