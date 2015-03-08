using DragonSpark.Extensions;
using DragonSpark.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Entity
{
	public class ObjectContextInstallationStep : IInstallationStep
	{
		/*readonly SecurityDataObjectContextFactory factory = new SecurityDataObjectContextFactory();*/

		public Type FactoryType { get; set; }

		public Type ObjectContextType { get; set; }

		public void Execute( DbContext context )
		{
			using ( var instance = CreateContext() )
			{
				Attach.Apply( instance.AddOrAttach );
				Remove.Apply( instance.DeleteObject );
				var result = instance.SaveChanges();
				DragonSpark.Runtime.Logging.Information( string.Format( "{0} saved '{1}' entities.", GetType().AssemblyQualifiedName, result ) );
			}
		}

		ObjectContext CreateContext()
		{
			var result = FactoryType.Transform( x => Activator.CreateInstance<IFactory>( x ).Create<ObjectContext>() ) ?? Activator.Create<ObjectContext>( ObjectContextType );
			return result;
		}

		public Collection<object> Attach
		{
			get { return attach ?? ( attach = new Collection<object>() ); }
		}	Collection<object> attach;

		public Collection<object> Remove
		{
			get { return remove ?? ( remove = new Collection<object>() ); }
		}	Collection<object> remove;
	}

	public class EntityInstallationStep : IInstallationStep
	{
		public void Execute( DbContext context )
		{
			Remove.Apply( y =>
			{
				var values = context.ResolveKeyValues( y );
				var current = context.Set( y.GetType() ).Find( values );
				current.NotNull( context.Remove );
			} );

			Attach.Apply( y =>
			{
				var ensureDefaults = EnsureDefaults( context, y );
				context.ApplyChanges( ensureDefaults );
			} );
		}

		static object EnsureDefaults( DbContext context, object item )
		{
			var type = item.GetType();
			var properties = context.ResolveNavigationProperties( type );
			var items = item.ToEnumerable( properties.SelectMany( x => Resolve( context, item, x ) ) ).NotNull();
			items.Apply( x => x.WithDefaults() );
			return item;
		}

		static IEnumerable<object> Resolve( DbContext context, object item, PropertyInfo propertyInfo )
		{
			var value = propertyInfo.GetValue( item, null );
			var entities = propertyInfo.PropertyType.GetCollectionElementType() != null ? EnsureCollection( context, value ) : EnsureValue( context, item, propertyInfo, value );
			var result = entities.NotNull().Select( x => EnsureDefaults( context, x ) );
			return result;
		}

		static IEnumerable<object> EnsureCollection( DbContext context, object value )
		{
			var result = value.To<IEnumerable>().Cast<object>();
			value.As<IList>( x => result.ToArray().Apply( y =>
			{
				var resolveKeyValues = context.ResolveKeyValues( y );
				var current = context.Set( y.GetType() ).Find( resolveKeyValues );
				current.NotNull( z =>
				{
					var index = x.IndexOf( y );
					x.Remove( y );
					x.Insert( index, z );
				} );
			} ) );
			return result;
		}

		static IEnumerable<object> EnsureValue( DbContext context, object item, PropertyInfo propertyInfo, object value )
		{
			var resolveKeyValues = context.ResolveKeyValues( value );
			var current = context.Set( value.GetType() ).Find( resolveKeyValues );
			current.NotNull( x => propertyInfo.SetValue( item, current, null ) );
			var result = current != null ? Enumerable.Empty<object>() : value.ToEnumerable();
			return result;
		}

		public Collection<object> Attach
		{
			get { return attach ?? ( attach = new Collection<object>() ); }
		}	Collection<object> attach;

		public Collection<object> Remove
		{
			get { return remove ?? ( remove = new Collection<object>() ); }
		}	Collection<object> remove;
	}
}