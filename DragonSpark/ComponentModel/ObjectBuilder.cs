using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DragonSpark.ComponentModel
{
	public interface IBuildPropertyRepository
	{
		IEnumerable<DefaultValueParameter> GetProperties( object target );

		void MarkBuilt( DefaultValueParameter property );
	}

	public class DefaultPropertyValueFactory : FactoryBase<DefaultValueParameter, object>
	{
		public static DefaultPropertyValueFactory Instance { get; } = new DefaultPropertyValueFactory();

		protected override object CreateItem( DefaultValueParameter parameter )
		{
			var result = parameter.Metadata.From<IDefaultValueProvider>( parameter.Instance ).With( provider => provider.GetValue( parameter ) ) 
				??
				parameter.Metadata.FromMetadata<DefaultValueAttribute, object>( attribute => attribute.Value );
			return result;
		}
	}

	public class DefaultValueParameter
	{
		public DefaultValueParameter( [Required]object instance, [Required]PropertyInfo metadata )
		{
			Instance = instance;
			Metadata = metadata;
		}

		public object Instance { get; }

		public PropertyInfo Metadata { get; }

		public DefaultValueParameter Assign( object value )
		{
			Metadata.SetValue( Instance, value );
			return this;
		}
	}

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

	[Synchronized]
	class BuildPropertyRepository : IBuildPropertyRepository
	{
		public static BuildPropertyRepository Instance { get; } = new BuildPropertyRepository();

		[Reference]
		readonly WeakCollection<object> built = new WeakCollection<object>();

		[Reference]
		readonly ConditionalWeakTable<object, ICollection<PropertyInfo>> properties = new ConditionalWeakTable<object, ICollection<PropertyInfo>>();

		public IEnumerable<DefaultValueParameter> GetProperties( object target )
		{
			var result = !built.Contains( target ) ? properties.GetValue( target, BuildablePropertyCollectionFactory.Instance.Create ).Select( info => new DefaultValueParameter( target, info ) ).ToArray() : Enumerable.Empty<DefaultValueParameter>();
			return result;
		}

		public void MarkBuilt( DefaultValueParameter property )
		{
			var collection = properties.GetValue( property.Instance, BuildablePropertyCollectionFactory.Instance.Create );
			collection.Remove( property.Metadata );
			collection.Any().IsFalse( () => built.Add( property.Instance ) );
		}
	}

	public class BuildablePropertyCollectionFactory : FactoryBase<object, ICollection<PropertyInfo>>
	{
		public static BuildablePropertyCollectionFactory Instance { get; } = new BuildablePropertyCollectionFactory();

		protected override ICollection<PropertyInfo> CreateItem( object parameter )
		{
			var result = parameter.GetType().GetRuntimeProperties()
				.Where( x => x.IsDecoratedWith<DefaultValueAttribute>() || x.IsDecoratedWith<DefaultValueBase>() )
				.ToList();
			return result;
		}
	}
}