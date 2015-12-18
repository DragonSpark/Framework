using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.ComponentModel
{
	public class ObjectBuilder : IObjectBuilder
	{
		readonly IBuildPropertyRepository repository;
		readonly IFactory<DefaultValueParameter, object> factory;
		public static ObjectBuilder Instance { get; } = new ObjectBuilder();

		public ObjectBuilder() : this( BuildPropertyRepository.Instance )
		{}

		public ObjectBuilder( IBuildPropertyRepository repository ) : this( repository, DefaultPropertyValueFactory.Instance )
		{}

		public ObjectBuilder( [Required]IBuildPropertyRepository repository, [Required]IFactory<DefaultValueParameter, object> factory )
		{
			this.repository = repository;
			this.factory = factory;
		}

		public object BuildUp( object target )
		{
			repository.GetProperties( target ).Each( property => factory.Create( property ).With( o =>
			{
				repository.MarkBuilt( property.Assign( o ) );
			} ) );
			return target;
		}
	}
}