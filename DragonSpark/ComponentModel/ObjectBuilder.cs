using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.TypeSystem;
using Nito.ConnectedProperties;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public interface IBuildPropertyRepository
	{
		IEnumerable<DefaultValueParameter> GetProperties( object instance );

		void MarkBuilt( DefaultValueParameter property );
	}

	public class DefaultPropertyValueFactory : FactoryBase<DefaultValueParameter, object>
	{
		public static DefaultPropertyValueFactory Instance { get; } = new DefaultPropertyValueFactory();

		readonly IFactory<MemberInfo, IDefaultValueProvider> factory;

		public DefaultPropertyValueFactory() : this( ReflectionSurrogateFactory<IDefaultValueProvider>.Instance )
		{}

		public DefaultPropertyValueFactory( IFactory<MemberInfo, IDefaultValueProvider> factory )
		{
			this.factory = factory;
		}

		protected override object CreateItem( DefaultValueParameter parameter )
		{
			var result = FromFactory( parameter ).With( provider => provider.GetValue( parameter ) )
				??
				parameter.Metadata.FromMetadata<DefaultValueAttribute, object>( attribute => attribute.Value );
			return result;
		}

		[Cache]
		IDefaultValueProvider FromFactory( DefaultValueParameter parameter )
		{
			return factory.Create( parameter.Metadata );
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

		protected bool Equals( DefaultValueParameter other )
		{
			return Equals( Instance, other.Instance ) && Equals( Metadata, other.Metadata );
		}

		public override bool Equals( object obj )
		{
			return !ReferenceEquals( null, obj ) && ( ReferenceEquals( this, obj ) || obj.GetType() == this.GetType() && Equals( (DefaultValueParameter)obj ) );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = Instance.GetHashCode() * 397 ^ Metadata.GetHashCode();
				return result;
			}
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

	public abstract class ConnectedValue<T> : WritableValue<T>
	{
		/*public static T Property<TProperty>( [Required]object @this ) where TProperty : ConnectedValue<T>
		{
			var value = (ConnectedValue<T>)typeof(TProperty).GetConstructor( typeof(object) ).Invoke( new[] { @this } );
			var result = value.Item;
			return result;
		}*/

		readonly Func<T> create;

		protected ConnectedValue( object instance, Type type, Func<T> create = null ) : this( instance, type.AssemblyQualifiedName, create )
		{}

		protected ConnectedValue( object instance, string name, Func<T> create = null ) : this( PropertyConnector.Default.Get( instance, name ).Cast<T>(), create )
		{}

		protected ConnectedValue( ConnectibleProperty<T> property, Func<T> create )
		{
			Property = property;
			this.create = create ?? ( () => default(T) );
		}

		public override void Assign( T item )
		{
			Property.Set( item );
		}

		public override T Item => Property.GetOrCreate( create );

		protected ConnectibleProperty<T> Property { get; }
	}

	/*public static class Properties
	{
		public static T Property<T>( [Required]object @this ) where T : IValue
		{
			var value = (T)typeof(T).GetConstructor( typeof(object) ).Invoke( new[] { @this } );
			var result = value.Item;
			return result;
		}
	}*/

	[Synchronized]
	class BuildPropertyRepository : IBuildPropertyRepository
	{
		public static BuildPropertyRepository Instance { get; } = new BuildPropertyRepository();

		class IsBuilt : ConnectedValue<bool>
		{
			public IsBuilt( object instance ) : base( instance, typeof(IsBuilt) )
			{}
		}

		class Properties : ConnectedValue<ICollection<PropertyInfo>>
		{
			public Properties( object instance ) : base( instance, typeof(Properties), () => BuildablePropertyCollectionFactory.Instance.Create( instance ) )
			{}
		}

		public IEnumerable<DefaultValueParameter> GetProperties( object instance )
		{
			var result = !new IsBuilt( instance ).Item ? new Properties( instance ).Item.Select( info => new DefaultValueParameter( instance, info ) ).ToArray() : Enumerable.Empty<DefaultValueParameter>();
			return result;
		}

		public void MarkBuilt( DefaultValueParameter property )
		{
			var collection = new Properties( property.Instance ).Item;
			collection.Remove( property.Metadata );
			collection.Any().IsFalse( () => new IsBuilt( property.Instance ).Assign( true ) );
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